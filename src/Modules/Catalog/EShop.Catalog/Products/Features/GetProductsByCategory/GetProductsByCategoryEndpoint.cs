using EShop.Catalog.Products.Features.GetProducts;
using EShop.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Products.Features.GetProductByCategory
{

    public record GetProductsByCategoryResponse(PaginationResult<ProductDto> Products);

    public class GetProductsByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (
                [AsParameters] PaginationRequest request,
                string category,
                ISender mediator) =>
            {
                var result = await mediator.Send(new GetProductsByCategoryQuery(request, category));

                var response = result.Adapt<GetProductsByCategoryResponse>();

                return Results.Ok(response);

            })
                .WithName("Get Products by Category")
                .Produces<GetProductsByCategoryResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get Products by Category")
                .WithDescription("Get Products by Category");

        }
    }
}
