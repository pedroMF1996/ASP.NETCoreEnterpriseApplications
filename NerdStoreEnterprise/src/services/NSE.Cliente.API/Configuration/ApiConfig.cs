using MediatR;
using Microsoft.EntityFrameworkCore;
using NSE.Cliente.API.Data;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Cliente.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.RegisterServiceConfiguration();

            services.AddMessageBusConfiguration(configuration);

            services.AddSwaggerConfiguration();

            services.AddJwtConfiguration(configuration);
           
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            // Add services to the container.
            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddDbContext<ClienteDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

                       

            services.AddCors(opt =>
            {
                opt.AddPolicy("Total",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });
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
            
            app.UseRouting();

            app.UseCors("Total");

            app.UseAuthConfiguration();
        }
    }
}
