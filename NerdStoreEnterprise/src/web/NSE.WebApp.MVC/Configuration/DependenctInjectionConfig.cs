using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependenctInjectionConfig
    {
        public static void AddRegisterServises(this IServiceCollection services)
        {
            services.AddHttpClient<IAutenticacaoService, AutenticacaoServise>();
        }
    }
}
