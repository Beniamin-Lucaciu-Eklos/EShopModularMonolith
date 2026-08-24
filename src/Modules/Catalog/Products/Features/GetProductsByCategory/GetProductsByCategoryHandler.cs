using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Products.Features.GetProducts
{
    public record GetProductsByCategoryQuery(string Category)
        : IQuery<GetProductsByCategoryResult>;

    public record GetProductsByCategoryResult(IEnumerable<ProductDto> Products);

    public class GetProductsByCategoryHandler(CatalogDbContext dbContext)
        : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
        {
            var products = await dbContext.Products
                .AsNoTracking()
                .Where(p => p.Categories.Contains(query.Category))
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            var productDtos = products.Adapt<List<ProductDto>>();

            return new GetProductsByCategoryResult(productDtos);
        }
    }
}
