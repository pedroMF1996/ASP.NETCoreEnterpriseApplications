using FluentValidation.Results;
using MediatR;
using NSE.Cliente.API.Application.Commands;
using NSE.Cliente.API.Application.Events;
using NSE.Cliente.API.Data;
using NSE.Cliente.API.Models;
using NSE.Core.MediatR;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Cliente.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
            services.AddScoped<IRequestHandler<AdicionarEnderecoCommand, ValidationResult>, ClienteCommandHandler>();

            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ClienteDbContext>();
        }
    }
}
