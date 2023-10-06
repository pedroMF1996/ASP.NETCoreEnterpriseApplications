using NSE.Cliente.API.Service;
using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Cliente.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnectionString("RabbitMQ"))
                .AddHostedService<RegistroClienteIntegrationHandler>();
        } 
    }
}
