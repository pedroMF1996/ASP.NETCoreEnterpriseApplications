using NSE.WebAPI.Core.Identidade;

namespace NSE.Identity.API.Configuration
{
    public static class APIConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services) {
            // Add services to the container.

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            return services;
        }
        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment) {
            app.UseISwaggerConfiguration(environment);

            app.UseHttpsRedirection();

            app.UseAuthConfiguration();

            return app;
        }
    }
}
