using NSE.Core.Data;

namespace NSE.Cliente.API.Models
{
    public interface IClienteRepository : IRepository<ClienteEntity>
    {
        Task Adicionar(ClienteEntity cliente);
        Task<IEnumerable<ClienteEntity>> ObterTodos();
        Task<ClienteEntity> ObterPorCpf(string cpf);
    }
}
