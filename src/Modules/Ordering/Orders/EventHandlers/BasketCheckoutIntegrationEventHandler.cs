using EShop.Ordering.Orders.Features.CreateOrder;
using MassTransit;
using Shared.Messaging.Events;

namespace EShop.Ordering.Orders.EventHandlers
{
    public class BasketCheckoutIntegrationEventHandler(
        IMediator mediator,
        ILogger<BasketCheckoutIntegrationEventHandler> logger)
        : IConsumer<BasketCheckoutIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
        {
            logger.LogInformation("BasketCheckoutIntegrationEventHandler: Consuming BasketCheckoutIntegrationEvent for User {UserName}", context.Message.UserName);

            var command = MapToCreateOrderCommand(context.Message);

            await mediator.Send(command);

            logger.LogInformation("BasketCheckoutIntegrationEventHandler: Order created for User {UserName} with OrderId {OrderId}", context.Message.UserName, command.Order.Id);
        }

        private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutIntegrationEvent message)
        {
            var addressDto = new AddressDto(
                message.FirstName,
                message.LastName,
                message.EmailAddress,
                message.AddressLine,
                message.Country,
                message.State,
                message.ZipCode);

            var paymentDto = new PaymentDto(
                message.CardName,
                message.CardNumber,
                message.Expiration,
                message.Cvv,
                message.PaymentMethod);

            var orderId = Guid.NewGuid();

            var oderDto = new OrderDto(
                orderId,
                message.CustomerId,
                message.UserName,
                addressDto,
                addressDto,
                paymentDto,
                Items: [
                    //real order dto item
                    new OrderItemDto(orderId,
                        new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                        2,500),

                        new OrderItemDto(orderId,
                        new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
                         1,400)
                ]);

            return new CreateOrderCommand(oderDto);
        }
    }
}
