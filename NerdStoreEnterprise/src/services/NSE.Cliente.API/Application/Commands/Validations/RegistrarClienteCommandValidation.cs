using FluentValidation;
using NSE.Core.DomainObjects;

namespace NSE.Cliente.API.Application.Commands.Validations
{
    public class RegistrarClienteCommandValidation : AbstractValidator<RegistrarClienteCommand>
    {
        public RegistrarClienteCommandValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente invalido");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Nome do cliente nao foi informado");

            RuleFor(c => c.Cpf)
                .Must(TerCpfValido)
                .WithMessage("CPF do cliente invalido");

            RuleFor(c => c.Email)
                .Must(TerEmailValido)
                .WithMessage("Email do cliente invalido");
        }

        private static bool TerCpfValido(string cpf)
        {
            return Cpf.Validar(cpf);
        }
        private static bool TerEmailValido(string email)
        {
            return Email.Validar(email);
        }
    }
}
