using EShop.Shared.Data.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Data.Seed
{
    public class CatalogDataSeeder(CatalogDbContext dbContext)
        : IDataSeeder
    {
        public async Task SeedAsync(CancellationToken cancellation = default)
        {
            if (!await dbContext.Products.AnyAsync(cancellation))
            {
                await dbContext.Products.AddRangeAsync(InitialData.Products, cancellation);
                await dbContext.SaveChangesAsync(cancellation);
            }
        }
    }
}
