using FluentValidation.Results;
using MediatR;
using NSE.Cliente.API.Application.Commands;
using NSE.Cliente.API.Data;
using NSE.Cliente.API.Models;
using NSE.Core.MediatR;

namespace NSE.Cliente.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServiceConfiguration( this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();


            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ClienteDbContext>();
        }
    }
}
