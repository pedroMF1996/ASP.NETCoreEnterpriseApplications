using NSE.Identity.API.Services;
using NSE.WebAPI.Core.Identidade;
using NSE.WebAPI.Core.Usuario;
using NSE.Core.Data;
using NSE.Identity.API.Data;

namespace NSE.Identity.API.Configuration
{
    public static class APIConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddScoped<AuthenticationService>();

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddMessageBusConfiguration(configuration);

            return services;
        }
        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseISwaggerConfiguration(environment);

            app.UseHttpsRedirection();

            app.UseAuthConfiguration();

            //localhost/jwks
            app.UseJwksDiscovery();

            app.UseEnsureDatabaseMigrations<ApplicationDBContext>();

            return app;
        }
    }
}
