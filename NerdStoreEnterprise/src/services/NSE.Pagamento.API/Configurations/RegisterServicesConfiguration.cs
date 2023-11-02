using NSE.Pagamento.API.Data;
using NSE.Pagamento.API.Data.Repository;
using NSE.Pagamento.API.Facade;
using NSE.Pagamento.API.Models;
using NSE.Pagamento.API.Services;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Pagamento.API.Configurations
{
    public static class RegisterServicesConfiguration
    {
        public static IServiceCollection AddRegisterServisesConfiguration(this IServiceCollection services)
        {

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IPagamentoFacace, PagamentoCartaoCreditoFacade>();
            services.AddScoped<IPagamentoService, PagamentoService>();

            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<PagamentosContext>();

            return services;
        }
    }
}
