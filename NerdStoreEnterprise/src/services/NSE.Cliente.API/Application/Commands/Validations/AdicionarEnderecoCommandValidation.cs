using FluentValidation;

namespace NSE.Cliente.API.Application.Commands.Validations
{
    public class AdicionarEnderecoCommandValidation : AbstractValidator<AdicionarEnderecoCommand>
    {
        public AdicionarEnderecoCommandValidation()
        {
            RuleFor(c => c.Logradouro)
                .NotEmpty()
                .WithMessage("Informe o Logradouro");

            RuleFor(c => c.Numero)
                .NotEmpty()
                .WithMessage("Informe o Número");

            RuleFor(c => c.Cep)
                .NotEmpty()
                .WithMessage("Informe o CEP");

            RuleFor(c => c.Bairro)
                .NotEmpty()
                .WithMessage("Informe o Bairro");

            RuleFor(c => c.Cidade)
                .NotEmpty()
                .WithMessage("Informe o Cidade");

            RuleFor(c => c.Estado)
                .NotEmpty()
                .WithMessage("Informe o Estado");
        }
    }
}
