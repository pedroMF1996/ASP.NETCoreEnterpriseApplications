using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Catalogo.API.Data;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Catalogo.API.Configuration
{
    public static class ApiConfig
    {

        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
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

            services.AddDbContext<CatalogoContext>(opt => opt.UseSqlServer(connectionString));

            services.AddMessageBusConfiguration(configuration);

            services.AddDependencyInjectionConfiguration(configuration);

            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddCors(opt =>
            {
                opt.AddPolicy("Total",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            services.AddJwtConfiguration(configuration);

            services.AddSwaggerConfiguration();
        }

        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseSwaggerConfiguration(environment);

            app.UseHttpsRedirection();

            app.UseAuthConfiguration();

            app.UseEnsureDatabaseMigrations<CatalogoContext>();
        }
    }
}
