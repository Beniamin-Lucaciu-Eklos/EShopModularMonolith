using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Basket.Basket.Features.GetBasket
{

    public record GetBasketResponse(ShoppingCartDto ShoppingCart);

    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetBasketQuery(userName));

                var response = result.Adapt<GetBasketResponse>();

                return Results.Ok(response);
            })
                .Produces<GetBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithName("GetBasketByUsername")
                .WithDescription("Get Basket By Username")
                .WithSummary("Get Basket by username")
                .RequireAuthorization();
        }
    }
}
