using NSE.Core.Utils;
using NSE.MessageBus;
using NSE.Pedido.API.Services;

namespace NSE.Pedido.API.Configurations
{
    public static class MessageBusConfiguration
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnectionString("RabbitMQ"))
                .AddHostedService<PedidoOrquestradorIntegrationHandler>() 
                .AddHostedService<PedidoIntegrationHandler>(); 
        }
    }
}
