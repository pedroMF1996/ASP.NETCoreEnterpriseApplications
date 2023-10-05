using NSE.Core.MediatR;
using NSE.Pedido.Domain.Voucher.Interface;
using NSE.Pedido.Infra.Data;
using NSE.Pedido.Infra.Data.Repository;

namespace NSE.Pedido.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddRegisterService(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<PedidosContext>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();

            return services;
        }
    }
}
