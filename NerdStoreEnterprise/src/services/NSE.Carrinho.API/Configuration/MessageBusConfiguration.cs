using NSE.Carrinho.API.Services;
using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Carrinho.API.Configurations
{
    public static class MessageBusConfiguration
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnectionString("RabbitMQ"))
                .AddHostedService<CarrinhoIntegrationHandler>();
        }
    }
}
