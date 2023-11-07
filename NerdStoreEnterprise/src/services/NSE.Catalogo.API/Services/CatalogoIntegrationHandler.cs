using NSE.Catalogo.API.Models;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.Integration;
using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Catalogo.API.Services
{
    public class CatalogoIntegrationHandler : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly IMessageBus _messageBus;

        public CatalogoIntegrationHandler(IServiceProvider serviceProvider, ILogger<CatalogoIntegrationHandler> logger, IMessageBus messageBus)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _messageBus = messageBus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private async void SetSubscribers()
        {
            await _messageBus.SubscribeAsync<PedidoAutorizadoIntegrationEvent>("PedidoAutorizado", async request => await BaixarEstoque(request));
        }

        private async Task BaixarEstoque(PedidoAutorizadoIntegrationEvent request)
        {
            using var scope = _serviceProvider.CreateScope();

            List<Produto> produtosComEstoque = new();

            var produtoRepository = scope.ServiceProvider.GetRequiredService<IProdutoRepository>();

            var idProdutos = string.Join(",", request.Itens.Select(c => c.Key));

            var produtos = await produtoRepository.ObterProdutosPorId(idProdutos);

            if (produtos.Count() != request.Itens.Count)
            {
                CancelarPedidoSemEstoque(request);
                return;
            }

            _logger.LogInformation($"Produtos buscados do catalogo com sucesso {DataHora.ObterFormatado()}");

            foreach (var produto in produtos)
            {
                var quantidadeProduto = request.Itens.FirstOrDefault(p => p.Key == produto.Id).Value;

                if (produto.EstaDisponivel(quantidadeProduto))
                {
                    _logger.LogInformation($"Produto {produto.Id} esta disponivel {DataHora.ObterFormatado()}");

                    produto.RetirarEstoque(quantidadeProduto);
                    produtosComEstoque.Add(produto);

                    _logger.LogInformation($"Produto {produto.Id} abatido do catalogo com sucesso {DataHora.ObterFormatado()}");
                }
            }

            if (produtosComEstoque.Count != request.Itens.Count)
            {
                CancelarPedidoSemEstoque(request);
                return;
            }

            foreach (var produto in produtosComEstoque)
                produtoRepository.Atualizar(produto);

            _logger.LogInformation($"Produtos atualizados no catalogo com sucesso {DataHora.ObterFormatado()}");

            if (!await produtoRepository.UnitOfWork.Commit())
                throw new DomainException($"Problemas ao atualizar estoque do pedido {request.Id}");

            _logger.LogInformation($"Produtos persistidos no catalogo com sucesso {DataHora.ObterFormatado()}");

            var pedidoBaixado = new PedidoBaixadoEstoqueIntegrationEvent(request.ClienteId, request.Id);

            await _messageBus.PublishAsync(pedidoBaixado);
        }

        private async void CancelarPedidoSemEstoque(PedidoAutorizadoIntegrationEvent request) =>
            await _messageBus.PublishAsync(new CancelarPedidoSemEstoqueIntegrationEvent(request.ClienteId, request.Id));
    }
}
