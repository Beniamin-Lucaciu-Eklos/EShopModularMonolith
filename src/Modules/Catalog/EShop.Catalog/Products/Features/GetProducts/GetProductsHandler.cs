using EShop.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Products.Features.GetProducts
{
    public record GetProductsQuery(PaginationRequest PaginationRequest)
        : IQuery<GetProductsResult>;

    public record GetProductsResult(PaginationResult<ProductDto> Products);

    public class GetProductsHandler(CatalogDbContext dbContext)
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            (int pageIndex, int pageSize) = query.PaginationRequest;

            var productsQuery = dbContext.Products
                .AsNoTracking()
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .OrderBy(p => p.Name)
                .AsQueryable();

            var totalCount = await productsQuery.LongCountAsync(cancellationToken);
            var products = await productsQuery.ToListAsync(cancellationToken);           
            var productDtos = products.Adapt<List<ProductDto>>();

            return new GetProductsResult(
                new PaginationResult<ProductDto>(pageIndex,
                    pageSize,
                    totalCount,
                    productDtos));
        }
    }
}
