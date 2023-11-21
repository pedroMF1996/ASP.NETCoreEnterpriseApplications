using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NSE.Core.Data
{
    public static class DatabaseMigrationExtension
    {
        public static void UseEnsureDatabaseMigrations<T>(this IApplicationBuilder app) where T : DbContext
        {
            var dataBaseContext = app.ApplicationServices.CreateScope()
                                        .ServiceProvider.GetRequiredService<T>();

            dataBaseContext.Database.Migrate();
        }
    }
}
