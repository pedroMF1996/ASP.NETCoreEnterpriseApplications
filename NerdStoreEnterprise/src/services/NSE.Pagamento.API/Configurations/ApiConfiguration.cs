using NSE.WebAPI.Core.Identidade;

namespace NSE.Pagamento.API.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerConfiguration();
            services.AddJwtConfiguration(configuration);


            services.AddCors(opt => 
                opt.AddPolicy("total", builder => 
                                        builder.AllowAnyHeader()
                                                .AllowAnyMethod()   
                                                .AllowAnyOrigin()));

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseSwaggerConfiguration(environment);
            app.UseCors("total");
            return app;
        }
    }
}
