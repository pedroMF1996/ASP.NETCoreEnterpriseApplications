using NSE.BFF.Compras.Extensions;
using NSE.BFF.Compras.Services;
using NSE.WebAPI.Core.Polly;
using NSE.WebAPI.Core.Usuario;
using Polly;

namespace NSE.BFF.Compras.Configurations
{
    public static class DependencyInjectionsConfigurations
    {
        public static IServiceCollection AddRegisterServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddTransient<HttpCientAuthorizationDelegateHandler>();

            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpCientAuthorizationDelegateHandler>()
                .AddPolicyHandler(PolicyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<ICarrinhoService, CarrinhoService>()
                .AddHttpMessageHandler<HttpCientAuthorizationDelegateHandler>()
                .AddPolicyHandler(PolicyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            return services;
        }
    }
    
}
