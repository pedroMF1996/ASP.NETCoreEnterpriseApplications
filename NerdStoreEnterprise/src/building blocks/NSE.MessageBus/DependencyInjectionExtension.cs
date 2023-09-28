using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
