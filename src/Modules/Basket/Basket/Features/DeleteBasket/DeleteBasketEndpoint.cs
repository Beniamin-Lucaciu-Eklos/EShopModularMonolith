using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Basket.Basket.Features.DeleteBasket
{
    public record DeleteBasketResponse(bool IsSuccess);

    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteBasketCommand(userName));

                var response = result.Adapt<DeleteBasketResponse>();

                return Results.Ok(response);
            })
                .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithName("Delete basket")
                .WithDescription("Delete basket")
                .WithSummary("Delete basket")
                .RequireAuthorization();
        }
    }
}
