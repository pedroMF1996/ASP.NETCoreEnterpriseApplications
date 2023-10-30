using Microsoft.EntityFrameworkCore;
using NSE.Pagamento.API.Data;
using NSE.Pagamento.API.Facade;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Pagamento.API.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddDbContext<PagamentosContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.Configure<PagamentoConfig>(configuration.GetSection("PagamentoConfig"));

            services.AddRegisterServisesConfiguration();
         
            //services.AddSwaggerConfiguration();
            services.AddJwtConfiguration(configuration);
            services.AddMessageBusConfiguration(configuration);

            services.AddCors(opt => 
                opt.AddPolicy("total", builder => 
                                        builder.AllowAnyHeader()
                                                .AllowAnyMethod()   
                                                .AllowAnyOrigin()));

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseHttpsRedirection();
            //app.UseSwaggerConfiguration(environment);
            app.UseAuthConfiguration();
            app.UseCors("total");
            return app;
        }
    }
}
