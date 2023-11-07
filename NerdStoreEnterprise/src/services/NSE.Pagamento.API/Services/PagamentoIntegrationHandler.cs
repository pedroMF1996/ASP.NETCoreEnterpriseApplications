using EasyNetQ;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using NSE.Pagamento.API.Models;

namespace NSE.Pagamento.API.Services
{

    public class PagamentoIntegrationHandler : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageBus _messageBus;

        public PagamentoIntegrationHandler(IMessageBus messageBus, IServiceProvider serviceProvider)
        {

            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            _messageBus.AdvancedBus.Connected += OnConnect;

            SetSubscribers();

            return Task.CompletedTask;
        }

        private async void SetResponder()
        {
            await _messageBus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(
                async request => await AutorizarPagamento(request));
        }

        private async void SetSubscribers()
        {
            await _messageBus.SubscribeAsync<PedidoBaixadoEstoqueIntegrationEvent>("PedidoBaixadoEstoque",
                async request => await CapturarPagamento(request));
            await _messageBus.SubscribeAsync<CancelarPedidoSemEstoqueIntegrationEvent>("PedidoBaixadoEstoque",
                async request => await CancelarPagamento(request));
        }

        private async Task CancelarPagamento(CancelarPedidoSemEstoqueIntegrationEvent request)
        {
            using var scope = _serviceProvider.CreateScope();

            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

            var response = await pagamentoService.CancelarPagamento(request.Id);

            if (!response.ValidationResult.IsValid)
                throw new DomainException($"Falha ao cancelar pagamento do pedido {request.Id}");

        }

        private async Task CapturarPagamento(PedidoBaixadoEstoqueIntegrationEvent request)
        {
            using var scope = _serviceProvider.CreateScope();

            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

            var response = await pagamentoService.CapturarPagamento(request.Id);

            if (!response.ValidationResult.IsValid)
                throw new DomainException($"Falha ao capturar pagamento do pedido {request.Id}");

            await _messageBus.PublishAsync(new PedidoPagoIntegrationEvent(request.Id, request.ClienteId));
        }

        private async Task<ResponseMessage> AutorizarPagamento(PedidoIniciadoIntegrationEvent message)
        {
            ResponseMessage responseMessage;

            using (var scope = _serviceProvider.CreateScope())
            {
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

                Models.Pagamento pagamento = new()
                {
                    PedidoId = message.PedidoId,
                    TipoPagamento = (TipoPagamento)message.TipoPagamento,
                    Valor = message.Valor,
                    CartaoCredito = new(message.NomeCartao, message.NumeroCartao, message.MesAnoVencimento, message.CVV)
                };

                responseMessage = await pagamentoService.AutorizarPagamento(pagamento);
            }

            return responseMessage;
        }
        private void OnConnect(object? sender, ConnectedEventArgs e)
        {
            Task.Run(SetResponder);
        }
    }
}
