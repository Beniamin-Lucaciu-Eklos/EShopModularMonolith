using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Products.Features.GetProducts
{
    public record GetProductByIdQuery(Guid Id)
        : IQuery<GetProductByIdResult>;

    public record GetProductByIdResult(ProductDto Product);

    public class GetProductByIdHandler(CatalogDbContext dbContext)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == query.Id);

            if (product is null)
                throw new ArgumentNullException($"Product not found {query.Id}");

            var productDto = product.Adapt<ProductDto>();

            return new GetProductByIdResult(productDto);
        }
    }
}
