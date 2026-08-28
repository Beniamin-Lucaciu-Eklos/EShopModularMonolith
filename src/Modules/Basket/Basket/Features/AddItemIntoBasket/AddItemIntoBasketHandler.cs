namespace EShop.Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem)
    : ICommand<AddItemIntoBasketResult>;

public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Username is required");

        RuleFor(x => x.ShoppingCartItem.ProductId)
         .NotEmpty()
         .WithMessage("ShoppingCartItem ProductId is empty");

        RuleFor(x => x.ShoppingCartItem.Quantity)
         .GreaterThan(0)
         .WithMessage("ShoppingCartItem quantity must be greater than 0");
    }
}


internal class AddItemIntoBasketHandler(BasketDbContext dbContext) :
    ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command,
        CancellationToken cancellationToken)
    {
        var shoppingCart = await dbContext.ShoppingCarts
            .AsNoTracking()
            .Include(sc => sc.Items)
            .FirstOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

        if (shoppingCart is null)
            throw new ShoppingCartNotFoundException(command.UserName);

        var shoppingItem = command.ShoppingCartItem;
        shoppingCart.AddItem(
            shoppingItem.Id,
            shoppingItem.Quantity,
            shoppingItem.Color,
            shoppingItem.Price,
            shoppingItem.ProductName);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddItemIntoBasketResult(shoppingCart.Id);
    }
}
