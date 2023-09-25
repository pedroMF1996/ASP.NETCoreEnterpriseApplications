using Microsoft.EntityFrameworkCore;

namespace NSE.Cliente.API.Data
{
    public class ClienteDbContext : DbContext
    {
        public ClienteDbContext(DbContextOptions<ClienteDbContext> opt) : base(opt) { }
        
    }
}
