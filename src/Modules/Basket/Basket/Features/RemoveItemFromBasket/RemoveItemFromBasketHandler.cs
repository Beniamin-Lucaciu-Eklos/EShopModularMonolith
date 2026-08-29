namespace EShop.Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(string UserName, Guid ProductId)
    :ICommand<RemoveItemFromBasketResult>;

public record RemoveItemFromBasketResult(Guid id);

public class RemoveItemFromBasketValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("UserName is required");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductOId is required");
    }
}

public class RemoveItemFromBasketHandler(BasketDbContext dbContext) :
    ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command,
        CancellationToken cancellationToken)
    {
        var shoppingCart = await dbContext.ShoppingCarts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

        if (shoppingCart is null)
            throw new ShoppingCartNotFoundException(command.UserName);

        shoppingCart.RemoveItem(command.ProductId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new RemoveItemFromBasketResult(shoppingCart.Id);
    }
}
