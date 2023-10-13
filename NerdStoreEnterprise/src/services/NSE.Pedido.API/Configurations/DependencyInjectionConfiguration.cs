using NSE.Core.MediatR;
using NSE.Pedido.API.Application.Queries;
using NSE.Pedido.Infra.Data;
using NSE.Pedido.Infra.Data.Repository;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Pedido.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddRegisterService(this IServiceCollection services)
        {
            #region API

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            #endregion
            #region Application

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IVoucherQuery, VoucherQuery>();

            #endregion           
            #region Events

            services.AddScoped<INotificationHandler<PedidoRealizadoEvent>, PedidoEventHandler>();

            #endregion
            #region Commands

            services.AddScoped<IRequestHandler<AdicionarPedidoCommand, ValidationResult>, PedidoCommandHandler>();

            #endregion
            #region Data

            services.AddScoped<PedidosContext>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();

            #endregion
            return services;
        }
    }
}
