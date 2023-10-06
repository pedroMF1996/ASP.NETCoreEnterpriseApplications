using NSE.MessageBus;
using NSE.Core.Utils;

namespace NSE.Pedido.API.Configurations
{
    public static class MessageBusConfiguration
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnectionString("RabbitMQ"));
        }
    }
}
