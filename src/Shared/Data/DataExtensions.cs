using EShop.Shared.Data.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Shared.Data
{
    public static class DataExtensions
    {
        public static IApplicationBuilder UseMigration<TDbContext>(this IApplicationBuilder app)
            where TDbContext : DbContext
        {
            MigrateDatabase<TDbContext>(app.ApplicationServices);

            SeedDatabase(app.ApplicationServices);

            return app;
        }

        private static void MigrateDatabase<TDbContext>
            (IServiceProvider serviceProvider)
            where TDbContext : DbContext
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

                context.Database.Migrate();
            }
        }
        private static void SeedDatabase
            (IServiceProvider serviceProvider)
        {
            _ = Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
                try
                {
                    foreach (var seeder in seeders)
                    {
                        await seeder.SeedAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }
    }
}
