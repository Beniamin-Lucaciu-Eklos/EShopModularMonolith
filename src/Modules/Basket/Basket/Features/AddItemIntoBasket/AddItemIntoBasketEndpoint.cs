namespace EShop.Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketRequest(string UserName, ShoppingCartItemDto ShoppingCartItem);

public record AddItemIntoBasketResponse(Guid Id);

public class AddItemIntoBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{userName}/items",
            async ([FromRoute] string userName,
            [FromBody] AddItemIntoBasketRequest request,
            IMediator mediator) =>
        {
            var command = new AddItemIntoBasketCommand(userName, request.ShoppingCartItem);

            var result = await mediator.Send(command);

            var response = result.Adapt<AddItemIntoBasketResponse>();

            return Results.Created($"/basket/{response.Id}", response);
        })
            .RequireAuthorization();
    }
}
