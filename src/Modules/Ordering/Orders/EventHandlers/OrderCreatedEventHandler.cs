
using MassTransit;

namespace EShop.Ordering.Orders.EventHandlers;

public class OrderCreatedEventHandler(
    ILogger<OrderCreatedEventHandler> logger,
    IBus bus)
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("OrderCreatedEventHandler: Order created with ID {OrderId}", notification.GetType().AssemblyQualifiedName);
        return Task.CompletedTask;
    }
}
