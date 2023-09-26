using FluentValidation.Results;
using MediatR;
using NSE.Cliente.API.Models;
using NSE.Core.Messages;

namespace NSE.Cliente.API.Application.Commands
{
    public class ClienteCommandHandler : CommandHandler, IRequestHandler<RegistrarClienteCommand, ValidationResult>
    {
        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {
            if (message.EhValido())
                return message.ValidationResult;

            var cliente = new ClienteEntity(message.Id, message.Name, message.Email, message.Cpf);



            if (true)//ja existe o cliente com o cpf informado
            {
                AdicionarErro("Este CPF ja esta em uso");
                return ValidationResult;
            }

        }
    }
}
