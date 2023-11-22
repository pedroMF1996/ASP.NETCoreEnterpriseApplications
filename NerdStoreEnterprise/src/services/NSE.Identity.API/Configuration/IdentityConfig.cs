using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Jwa;
using NSE.Identity.API.Data;
using NSE.Identity.API.Extensions;

namespace NSE.Identity.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
                                                                  IConfiguration configuration,
                                                                  IWebHostEnvironment environment)
        {

            services.AddDbContext<ApplicationDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            var appSettingsToken = configuration.GetSection("AppSettingsToken");
            services.Configure<AppTokenSettings>(appSettingsToken);

            services.AddJwksManager(opt => opt.Jws = Algorithm.Create(DigitalSignaturesAlgorithm.EcdsaSha256))
                .PersistKeysToDatabaseStore<ApplicationDBContext>()
                .UseJwtValidation();

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();

            //interpretar JWT
            //services.AddJwtConfiguration(configuration);

            return services;
        }
    }
}
