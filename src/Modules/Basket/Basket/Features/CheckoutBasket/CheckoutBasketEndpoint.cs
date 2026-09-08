namespace EShop.Basket.Basket.Features.CheckoutBasket;

public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckout);

public record CheckoutBasketResponse(bool IsSuccess);

public class CheckoutBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout",
            async (CheckoutBasketRequest request, IMediator mediator) =>
        {
            var command =  request.Adapt<CheckoutBasketCommand>();

            var result = await mediator.Send(command);

            var response = result.Adapt<CheckoutBasketResponse>();

            return Results.Ok(response);
        })
            .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithName("CheckoutBasket")
             .WithSummary("Checkout basket")
             .WithDescription("Checkout basket")
             .RequireAuthorization();
    }
}
