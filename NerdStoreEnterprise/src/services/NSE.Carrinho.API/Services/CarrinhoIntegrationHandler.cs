using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;

namespace NSE.Carrinho.API.Services
{
    public class CarrinhoIntegrationHandler : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageBus _messageBus;

        public CarrinhoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus messageBus)
        {
            _serviceProvider = serviceProvider;
            _messageBus = messageBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
        }

        private void SetSubscribers()
        {
            _messageBus.SubscribeAsync<PedidoRealizadoIntegrationEvent>("PedidoRealizado", async request =>
                await ApagarCarrinho(request));
        }

        private async Task ApagarCarrinho(PedidoRealizadoIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CarrinhoContext>();

                var carrinho = await context.CarrinhoCliente
                                                .Include(x => x.Itens)
                                                .FirstOrDefaultAsync(c => c.ClienteId == message.ClienteId);

                if (carrinho != null && carrinho.Itens.Count > 0)
                {
                    context.CarrinhoCliente.Remove(carrinho);
                    await context.SaveChangesAsync();
                }
            }
        }

    }
}
