using NSE.Core.DomainObjects;
using NSE.Core.Messages.Integration;
using NSE.Core.Utils;
using NSE.MessageBus;
using NSE.Pedido.Domain.Pedidos;

namespace NSE.Pedido.API.Services
{
    public class PedidoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public PedidoIntegrationHandler(IMessageBus messageBus, IServiceProvider serviceProvider, ILogger logger)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private async void SetSubscribers()
        {
            await _messageBus.SubscribeAsync<CancelarPedidoSemEstoqueIntegrationEvent>("PedidoCancelado", async request => 
                await CancelarPedido(request) );
            await _messageBus.SubscribeAsync<PedidoPagoIntegrationEvent>("PedidoPago", async request => 
                await FinalizarPedido(request));
        }

        private async Task FinalizarPedido(PedidoPagoIntegrationEvent request)
        {
            using var scope = _serviceProvider.CreateScope();

            var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

            var pedido = await pedidoRepository.ObterPorId(request.Id);

            _logger.LogInformation($"Pedido obtido com sucesso {DataHora.ObterFormatado()}");

            pedido.FinalizarPedido();

            pedidoRepository.Atualizar(pedido);

            if (! await pedidoRepository.UnitOfWork.Commit())
                throw new DomainException("Problemas ao finalizar o pedido");

            _logger.LogInformation($"Pedido finalizado com sucesso {DataHora.ObterFormatado()}");
        }

        private async Task CancelarPedido(CancelarPedidoSemEstoqueIntegrationEvent request)
        {
            using var scope = _serviceProvider.CreateScope();

            var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

            var pedido = await pedidoRepository.ObterPorId(request.Id);

            _logger.LogInformation($"Pedido obtido com sucesso {DataHora.ObterFormatado()}");

            pedido.CancelarPedido();
            
            pedidoRepository.Atualizar(pedido);

            if (!await pedidoRepository.UnitOfWork.Commit())
                throw new DomainException("Problemas ao cancelar o pedido");

            _logger.LogInformation($"Pedido cancelado com sucesso {DataHora.ObterFormatado()}");
        }
    }
}
