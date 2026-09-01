using EShop.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Products.Features.GetProducts
{
    public record GetProductsByCategoryQuery(
        PaginationRequest PaginationRequest,
        string Category)
        : IQuery<GetProductsByCategoryResult>;

    public record GetProductsByCategoryResult(PaginationResult<ProductDto> Products);

    public class GetProductsByCategoryHandler(CatalogDbContext dbContext)
        : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
        {
            (int pageIndex, int pageSize) = query.PaginationRequest;
            var productsQuery =
                dbContext.Products
                .AsNoTracking()
                .Where(p => p.Categories.Contains(query.Category))
                .OrderBy(p => p.Name)
                .AsQueryable();

            var totalCount = await productsQuery.CountAsync(cancellationToken);
            var products = await productsQuery
                .ToListAsync(cancellationToken);

            var productDtos = products.Adapt<List<ProductDto>>();

            return new GetProductsByCategoryResult(
                new PaginationResult<ProductDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    productDtos));
        }
    }
}
