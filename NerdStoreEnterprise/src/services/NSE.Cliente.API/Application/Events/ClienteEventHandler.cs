using MediatR;

namespace NSE.Cliente.API.Application.Events
{
    public class ClienteEventHandler : INotificationHandler<ClienteRegistradoEvent>
    {
        public Task Handle(ClienteRegistradoEvent notification, CancellationToken cancellationToken)
        {
            //Enviar evento de confirmacao
            return Task.CompletedTask;
        }
    }
}
