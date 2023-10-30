using FluentValidation.Results;
using MediatR;
using NSE.Core.MediatR;
using NSE.Pedido.API.Application.Commands;
using NSE.Pedido.API.Application.Events;
using NSE.Pedido.API.Application.Queries;
using NSE.Pedido.Domain.Pedidos;
using NSE.Pedido.Domain.Vouchers.Interface;
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
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            #endregion
            #region Application

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IVoucherQuery, VoucherQuery>();
            services.AddScoped<IPedidoQueries, PedidoQueries>();

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
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            #endregion  
            return services;
        }
    }
}
