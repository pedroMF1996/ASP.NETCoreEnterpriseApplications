using System;

namespace NSE.Identity.API.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
            {
                Title = "NerdStore Enterprise Identity API",
                Description = "Essa API faz parte do curso ASP.NET Core Enterprise Applications",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact() { Name = "Pedro Falleiros", Email = "pmfrp@hotmail.com" },
                License = new Microsoft.OpenApi.Models.OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            }));

            return services;
        }

        public static IApplicationBuilder UseISwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            // Configure the HTTP request pipeline.
            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            return app;
        }
    }
}
