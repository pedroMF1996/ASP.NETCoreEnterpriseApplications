using NSE.WebAPI.Core.Usuario;

namespace NSE.BFF.Compras.Configurations
{
    public static class DependencyInjectionsConfigurations
    {
        public static IServiceCollection AddRegisterServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            return services;
        }
    }
}
