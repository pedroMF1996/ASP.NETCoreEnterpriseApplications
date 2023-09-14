namespace NSE.WebApp.MVC.Configuration
{
    public static class WebAppConfig
    {
        public static void AddWebAppConfig(this IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        public static void UseWebAppConfig(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            // Configure the HTTP request pipeline.
            if (!environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityConfig();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
