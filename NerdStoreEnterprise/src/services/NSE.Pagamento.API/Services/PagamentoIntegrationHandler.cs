using EasyNetQ;
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

            return Task.CompletedTask;
        }

        private void SetResponder()
        {
            _messageBus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request =>
            await AutorizarPagamento(request));
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
