namespace EShop.Basket.Basket.Features.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout)
    : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketValidator
    : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketValidator()
    {
        RuleFor(x => x.BasketCheckout)
            .NotNull()
            .WithMessage("Basket checkout cannot be null");

        RuleFor(x => x.BasketCheckout.UserName)
            .NotEmpty()
            .WithMessage("UserName cannot be empty");
    }
}

public class CheckoutBasketHandler(
    BasketDbContext dbContext,
    IBus bus) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        using (var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                var basket = await dbContext.ShoppingCarts
                    .Include(b => b.Items)
                    .FirstOrDefaultAsync(b => b.UserName == command.BasketCheckout.UserName, cancellationToken);

                if (basket is null)
                    throw new ShoppingCartNotFoundException(command.BasketCheckout.UserName);

                var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
                eventMessage.TotalPrice = basket.TotalPrice;

                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccurredOn = DateTime.UtcNow,
                    Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName,
                    Content = JsonSerializer.Serialize(eventMessage)
                };

                dbContext.OutboxMessages.Add(outboxMessage);

                dbContext.ShoppingCarts.Remove(basket);

                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new CheckoutBasketResult(true);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new CheckoutBasketResult(false);
            }
        }
    }
}
