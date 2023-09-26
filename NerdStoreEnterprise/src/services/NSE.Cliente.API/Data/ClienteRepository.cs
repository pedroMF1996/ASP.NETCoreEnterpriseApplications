using Microsoft.EntityFrameworkCore;
using NSE.Cliente.API.Models;
using NSE.Core.Data;

namespace NSE.Cliente.API.Data
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClienteDbContext _dbContext;
        public IUnitOfWork UnitOfWork => _dbContext;

        public ClienteRepository(ClienteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Adicionar(ClienteEntity cliente)
        {
            await _dbContext.Clientes.AddAsync(cliente);
        }

        public async Task<IEnumerable<ClienteEntity>> ObterTodos()
        {
            return await _dbContext.Clientes.ToListAsync();
        }

        public async Task<ClienteEntity> ObterPorCpf(string cpf)
        {
            return await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Cpf.Numero == cpf);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
