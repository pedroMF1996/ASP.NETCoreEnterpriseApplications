using Microsoft.Extensions.DependencyInjection;

namespace NSE.MessageBus
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, string connection)
        {

            if (string.IsNullOrWhiteSpace(connection)) throw new ArgumentNullException();

            services.AddSingleton<IMessageBus>(new MessageBus(connection));

            return services;
        }
    }
}
