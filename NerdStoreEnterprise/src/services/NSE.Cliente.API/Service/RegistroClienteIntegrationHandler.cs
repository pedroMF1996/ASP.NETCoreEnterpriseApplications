using EasyNetQ;
using FluentValidation.Results;
using NSE.Cliente.API.Application.Commands;
using NSE.Core.MediatR;
using NSE.Core.Messages.Integration;

namespace NSE.Cliente.API.Service
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        private IBus _bus;

        private readonly IServiceProvider _serviceProvider;

        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus = RabbitHutch.CreateBus("host=localhost:5672");

            var response = await _bus.Rpc.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(
                async request => new ResponseMessage(await RegistrarCliente(request)));
        }

        private async Task<ValidationResult> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var registrarClienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
            ValidationResult sucesso;

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

                sucesso = await mediatr.EnviarComando(registrarClienteCommand);
            }

            return sucesso;
        }
    }
}
