using NSE.Carrinho.API.Data;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Carrinho.API.Configuration
{
    public static class DependencyInjectionConfg
    {
        public static IServiceCollection AddRegisterServices(this IServiceCollection services)
        {

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<CarrinhoContext>();

            return services;
        }
    }
}
