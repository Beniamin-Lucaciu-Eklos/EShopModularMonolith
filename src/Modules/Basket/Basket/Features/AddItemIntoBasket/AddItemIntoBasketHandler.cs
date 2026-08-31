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

public class AddItemIntoBasketHandler(IBasketRepository basketRepository) :
    ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command,
        CancellationToken cancellationToken)
    {
        var shoppingCart = await basketRepository.GetBasket(command.UserName, false, cancellationToken);

        var shoppingItem = command.ShoppingCartItem;
        shoppingCart.AddItem(
            shoppingItem.ProductId,
            shoppingItem.Quantity,
            shoppingItem.Color,
            shoppingItem.Price,
            shoppingItem.ProductName);

        await basketRepository.SaveChangesAsync(command.UserName, cancellationToken);

        return new AddItemIntoBasketResult(shoppingCart.Id);
    }
}
