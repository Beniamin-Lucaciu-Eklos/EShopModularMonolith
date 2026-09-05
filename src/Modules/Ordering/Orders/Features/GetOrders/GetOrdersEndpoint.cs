using EShop.Shared.Pagination;

namespace EShop.Ordering.Orders.Features.GetOrders;

public record GetOrdersResponse(PaginationResult<OrderDto> Orders);

public class GetOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/", async
            ([AsParameters] PaginationRequest request,
            ISender mediator) =>
        {
            var results = await mediator.Send(new GetOrdersQuery(request));
            var response = results.Adapt<GetOrdersResponse>();

            return Results.Ok(response);
        })
             .WithName("GetOrders")
            .Produces<GetOrdersResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Orders")
            .WithDescription("Get Orders");
    }
}
