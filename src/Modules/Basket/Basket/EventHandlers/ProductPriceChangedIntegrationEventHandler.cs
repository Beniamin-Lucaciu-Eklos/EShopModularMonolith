using EShop.Basket.Basket.Features.UpdateItemPriceInBasket;
using MassTransit;
using Shared.Messaging.Events;

namespace EShop.Basket.Basket.EventHandlers;

public class ProductPriceChangedIntegrationEventHandler(
    IMediator mediator,
    ILogger<ProductPriceChangedIntegrationEventHandler> logger)
    : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        logger.LogInformation("Handling ProductPriceChangedIntegrationEvent for ProductId: {ProductId}", context.Message.ProductId);

        var command = new UpdateItemPriceInBasketCommand(
            context.Message.ProductId,
            context.Message.Price
        );
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
        {
            logger.LogError("Failed to update item price in basket for ProductId: {ProductId}", context.Message.ProductId);
        }

        logger.LogInformation("Price in basket for ProductId: {ProductId}", context.Message.ProductId);
    }
}
