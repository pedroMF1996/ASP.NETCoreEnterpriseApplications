using NSE.Catalogo.API.Services;
using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Catalogo.API.Configuration
{
    public static class MessageBusConfiguration
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnectionString("RabbitMQ"))
                .AddHostedService<CatalogoIntegrationHandler>();
        }
    }
}
