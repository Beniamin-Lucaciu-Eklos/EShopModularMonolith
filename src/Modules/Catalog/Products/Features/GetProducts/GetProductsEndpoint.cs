using EShop.Catalog.Products.Features.DeleteProduct;
using EShop.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Products.Features.GetProducts
{
    public record GetProductsResponse(PaginationResult<ProductDto> Products);

    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/", async
                ([AsParameters] PaginationRequest request,
                ISender mediator) =>
            {
                var results = await mediator.Send(new GetProductsQuery(request));

                var response = results.Adapt<GetProductsResponse>();

                return Results.Ok(response);
            })
                 .WithName("GetProducts")
                .Produces<GetProductsResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get Products")
                .WithDescription("Get Products");
        }
    }
}
