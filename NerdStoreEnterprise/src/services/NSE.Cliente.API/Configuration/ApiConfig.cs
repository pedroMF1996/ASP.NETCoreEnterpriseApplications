using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSE.Cliente.API.Data;

namespace NSE.Cliente.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ClienteDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.RegisterServiceConfiguration();
        }
        
        public static void UseApiConfiguration(this IApplicationBuilder app)
        {

        }
    }
}
