using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;

namespace NSE.Identity.API.Data
{
    public class ApplicationDBContext : IdentityDbContext, ISecurityKeyContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> opt) : base(opt)
        {

        }

        public DbSet<KeyMaterial> SecurityKeys { get; set; }
    }
}
