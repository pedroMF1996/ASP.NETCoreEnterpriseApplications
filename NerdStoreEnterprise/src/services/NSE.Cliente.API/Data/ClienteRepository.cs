using NSE.Cliente.API.Models;
using NSE.Core.Data;

namespace NSE.Cliente.API.Data
{
    public class ClienteRepository : IRepository<ClienteEntity>
    {
        private readonly ClienteDbContext _dbContext;
        public IUnitOfWork unitOfWork => _dbContext;

        public ClienteRepository(ClienteDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
