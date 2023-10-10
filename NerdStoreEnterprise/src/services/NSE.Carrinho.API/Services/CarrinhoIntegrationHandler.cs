using EasyNetQ;
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
            await SetSubscribers();
            _messageBus.AdvancedBus.Connected += OnConnect;
        }

        private void OnConnect(object? sender, ConnectedEventArgs e)
        {
            Task.Run(SetSubscribers);
        }

        private async Task SetSubscribers()
        {
            await _messageBus.SubscribeAsync<PedidoRealizadoIntegrationEvent>("PedidoRealizado", async request => 
                await ApagarCarrinho(request));
        }

        private async Task ApagarCarrinho(PedidoRealizadoIntegrationEvent pedidoRealizado)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CarrinhoContext>();

                var carrinho = await context.CarrinhoCliente.FirstOrDefaultAsync(c => c.ClienteId == pedidoRealizado.ClienteId);

                if (carrinho != null)
                {
                    context.CarrinhoCliente.Remove(carrinho);

                    await context.SaveChangesAsync();
                }
            }
        }

    }
}
