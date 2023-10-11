using FluentValidation.Results;
using MediatR;
using NSE.Cliente.API.Application.Events;
using NSE.Cliente.API.Models;
using NSE.Core.Messages;

namespace NSE.Cliente.API.Application.Commands
{
    public class ClienteCommandHandler : CommandHandler, 
                                         IRequestHandler<RegistrarClienteCommand, ValidationResult>,
                                         IRequestHandler<AdicionarEnderecoCommand, ValidationResult>
    {

        private readonly IClienteRepository _clienteRepository;

        public ClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido())
                return message.ValidationResult;

            var clienteExistente = await _clienteRepository.ObterPorCpf(message.Cpf);
            
            if (clienteExistente != null)
            {
                AdicionarErro("Este CPF ja esta em uso");
                return ValidationResult;
            }

            var cliente = new ClienteEntity(message.Id, message.Name, message.Email, message.Cpf);
            
            await _clienteRepository.Adicionar(cliente);

            cliente.AdicionarEvento(new ClienteRegistradoEvent(cliente.Id,cliente.Nome, cliente.Email.Endereco, cliente.Cpf.Numero));

            return await PersistirDados(_clienteRepository.UnitOfWork);
        }
            
        public async Task<ValidationResult> Handle(AdicionarEnderecoCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var endereco = new Endereco(message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.Cep, message.Cidade, message.Estado, message.ClienteId);
            await _clienteRepository.AdicionarEndereco(endereco);

            return await PersistirDados(_clienteRepository.UnitOfWork);
        }
    }
}
