using MediatR;
using Microsoft.EntityFrameworkCore;
using NSE.Pedido.Infra.Data;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Pedido.API.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRegisterService();
            services.AddJwtConfiguration(configuration);
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());


            services.AddDbContext<PedidosContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
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
