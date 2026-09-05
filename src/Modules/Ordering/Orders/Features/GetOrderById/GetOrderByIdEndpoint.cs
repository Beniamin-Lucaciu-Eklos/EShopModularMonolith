
namespace EShop.Ordering.Orders.Features.GetOrderById;


public record GetOrderyByIdResponse(OrderDto Order);

public class GetOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var result = await mediator.Send(new GetOrderByIdQuery(id));

            var response = result.Adapt<GetOrderyByIdResponse>();
            return Results.Ok(response);
        })
              .WithName("Get Order By Id")
              .Produces<GetOrderyByIdResponse>(StatusCodes.Status201Created)
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .WithSummary("Get Order By Id")
              .WithDescription("Get Order By Id");
    }
}
