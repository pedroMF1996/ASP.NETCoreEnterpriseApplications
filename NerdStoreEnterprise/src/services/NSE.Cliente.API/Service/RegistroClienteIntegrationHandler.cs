using EasyNetQ;
using FluentValidation.Results;
using NSE.Cliente.API.Application.Commands;
using NSE.Core.MediatR;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;

namespace NSE.Cliente.API.Service
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;

        private readonly IServiceProvider _serviceProvider;

        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(
                async request => await RegistrarCliente(request));
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var registrarClienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
            ValidationResult sucesso;

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

                sucesso = await mediatr.EnviarComando(registrarClienteCommand);
            }

            return new ResponseMessage(sucesso);
        }
    }
}
