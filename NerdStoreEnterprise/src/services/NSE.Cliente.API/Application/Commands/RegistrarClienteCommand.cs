using NSE.Core.Messages;

namespace NSE.Cliente.API.Application.Commands
{
    public class RegistrarClienteCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }

        public RegistrarClienteCommand(Guid id, string name, string email, string cpf)
        {
            Id = id;
            AggregateId = id;
            Name = name;
            Email = email;
            Cpf = cpf;
        }

        public override bool EhValido()
        {
            return base.EhValido();
        }
    }
}
