using NSE.BFF.Compras.Extensions;
using NSE.WebAPI.Core.Identidade;

namespace NSE.BFF.Compras.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRegisterServiceConfiguration(configuration);
            services.AddJwtConfiguration(configuration);
            services.AddSwaggerConfiguration();
            services.AddMessageBusConfiguration(configuration);


            services.AddGrpcConfig(configuration);

            services.Configure<AppServiceSettings>(configuration);

            services.AddCors(opt =>
            {
                opt.AddPolicy("Total", builder =>
                                            builder.AllowAnyOrigin()
                                                   .AllowAnyMethod()
                                                   .AllowAnyHeader());
            });

            // Add services to the container.
            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseSwaggerConfiguration(environment);
            // Configure the HTTP request pipeline.
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Total");

            app.UseAuthConfiguration();

            return app;
        }
    }
}
