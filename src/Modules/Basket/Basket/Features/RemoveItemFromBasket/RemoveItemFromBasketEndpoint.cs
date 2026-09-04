
namespace EShop.Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketResponse(Guid id);

public class RemoveItemFromBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}/items/{productId}",
            async ([FromRoute] string userName,
                [FromRoute] Guid productId,
                IMediator mediator) =>
        {
            var command = new RemoveItemFromBasketCommand(userName, productId);

            var result = await mediator.Send(command);

            var response = result.Adapt<RemoveItemFromBasketResponse>();

            return TypedResults.Ok(response);
        })
            .Produces<RemoveItemFromBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Remove Item From Basket")
            .WithDescription("Remove Item from Basket")
            .RequireAuthorization();
    }
}
