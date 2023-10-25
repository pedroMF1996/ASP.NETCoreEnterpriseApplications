using Microsoft.OpenApi.Models;
using NSE.WebAPI.Core.Swagger;

namespace NSE.BFF.Compras.Configurations
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "NerdStore Enterprise Compras BFF",
                    Description = "Esta API faz parte do curso ASP.NET Core Enterprise Application",
                    Contact = new OpenApiContact() { Name = "Pedro Martins Falleiros", Email = "pmfrp@hotmail.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/license/MIT") }
                });

                c.AddSwaggerJwtAuthorizationConfiguration();
            });
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }
        }
    }
}
