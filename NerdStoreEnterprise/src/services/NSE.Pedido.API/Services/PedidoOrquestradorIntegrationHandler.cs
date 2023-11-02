﻿using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using NSE.Pedido.API.Application.Queries;

namespace NSE.Pedido.API.Services
{
    public class PedidoOrquestradorIntegrationHandler : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;

        public PedidoOrquestradorIntegrationHandler(ILogger logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Servico de pedidos iniciado");

            _timer = new Timer(ProcessarPedidos, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        private async void ProcessarPedidos(object state)
        {
            _logger.LogInformation("Processando pedidos");

            using var scope = _serviceProvider.CreateScope();

            var pedidosQueries = scope.ServiceProvider.GetRequiredService<IPedidoQueries>();
            var pedido = await pedidosQueries.ObterPedidosAutorizados();

            if (pedido == null)
                return;

            var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

            var pedidoAutorizado = new PedidoAutorizadoIntegrationEvent(pedido.ClienteId,
                                                                        pedido.Id,
                                                                        pedido.PedidoItems.ToDictionary(p => p.ProdutoId, p => p.Quantidade));

            await bus.PublishAsync(pedidoAutorizado);

            _logger.LogInformation($"O Pedido ID: {pedido.Id} foi encaminhado para baixa no estoque.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Servico de pedidos finalizado");

            _timer?.Change(Timeout.Infinite, 0);
            Dispose();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
