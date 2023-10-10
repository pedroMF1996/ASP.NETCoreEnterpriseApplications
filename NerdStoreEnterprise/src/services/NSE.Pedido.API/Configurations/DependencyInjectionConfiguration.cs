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

            // API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            // Application
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IVoucherQuery, VoucherQuery>();
            services.AddScoped<IPedidoQueries, PedidoQueries>();

            //Events
            services.AddScoped<INotificationHandler<PedidoRealizadoEvent>, PedidoEventHandler>();

            //Commands
            services.AddScoped<IRequestHandler<AdicionarPedidoCommand, ValidationResult>, PedidoCommandHandler>();

            // Data
            services.AddScoped<PedidosContext>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            return services;
        }
    }
}
