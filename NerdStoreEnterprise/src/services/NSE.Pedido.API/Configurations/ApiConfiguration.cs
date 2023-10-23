using MediatR;
using Microsoft.EntityFrameworkCore;
using NSE.Pedido.Infra.Data;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Pedido.API.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddRegisterService();
            services.AddControllers();
            services.AddEndpointsApiExplorer();


            string connectionString = "";
            if (environment.IsProduction())
            {
                connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "";
            }
            if (environment.IsDevelopment())
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            services.AddDbContext<PedidosContext>(opt => opt.UseSqlServer(connectionString));

            services.AddJwtConfiguration(configuration);
            services.AddMessageBusConfiguration(configuration);
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

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

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            // Configure the HTTP request pipeline.
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerConfiguration(environment);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Total");

            app.UseAuthConfiguration();

            return app;
        }
    }
}
