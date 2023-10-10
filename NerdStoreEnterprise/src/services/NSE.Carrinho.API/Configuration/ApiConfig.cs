using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Configurations;
using NSE.Carrinho.API.Data;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Carrinho.API.Configuration
{
    public static class ApiConfig
    {

        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRegisterServices();

            // Add services to the container.

            services.AddDbContext<CarrinhoContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            
            services.AddJwtConfiguration(configuration);
            services.AddMessageBusConfiguration(configuration);

            services.AddSwaggerConfiguration();
            
            services.AddCors(opt =>
            {
                opt.AddPolicy("Total",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });
            
            return services;
        }

        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            // Configure the HTTP request pipeline.
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerConfiguration(environment);
            
            app.UseHttpsRedirection();
            
            app.UseCors("Total");

            app.UseAuthConfiguration();
        }
    }
}
