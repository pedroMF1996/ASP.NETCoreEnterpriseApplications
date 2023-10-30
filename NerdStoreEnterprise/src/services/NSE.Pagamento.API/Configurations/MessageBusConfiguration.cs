using NSE.Core.Utils;
using NSE.MessageBus;
using NSE.Pagamento.API.Services;

namespace NSE.Pagamento.API.Configurations
{
    public static class MessageBusConfiguration
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnectionString("RabbitMQ")).
                AddHostedService<PagamentoIntegrationHandler>();
        }
    }
}
