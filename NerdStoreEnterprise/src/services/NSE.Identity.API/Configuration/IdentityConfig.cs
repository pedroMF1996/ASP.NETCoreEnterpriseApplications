using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSE.Identity.API.Data;
using NSE.Identity.API.Extensions;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Identity.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            string connectionString = "";
            if (environment.IsProduction())
            {
                connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "";
            }
            if (environment.IsDevelopment())
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            services.AddDbContext<ApplicationDBContext>(opt => opt.UseSqlServer(connectionString));


            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();


            //JWT
            services.AddJwtConfiguration(configuration);

            return services;
        }
    }
}
