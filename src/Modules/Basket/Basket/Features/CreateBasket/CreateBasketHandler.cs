namespace EShop.Basket.Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCart) : ICommand<CreateBasketResult>;

public record CreateBasketResult(Guid id);

public class CreateProductCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.ShoppingCart.UserName)
            .NotEmpty()
            .WithMessage("UserName is required");

        //RuleFor(p => p.ShoppingCart)
        //    .NotEqual(Guid.Empty)
        //    .WithMessage("ShoppingCart is empty");
    }
}
public class CreateBasketHandler
    : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    private readonly IBasketRepository _basketRepository;
    private readonly ILogger<CreateBasketHandler> _logger;

    public CreateBasketHandler(
        IBasketRepository basketRepository,
        ILogger<CreateBasketHandler> logger)
    {
        _basketRepository = basketRepository;
        _logger = logger;
    }

    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("start basket creating");

        var shoppingCart = CreateNewShoppingCart(command.ShoppingCart);

        await _basketRepository.CreateBasket(shoppingCart, cancellationToken);

        return new CreateBasketResult(shoppingCart.Id);
    }

    private ShoppingCart CreateNewShoppingCart(ShoppingCartDto shoppingCartDto)
    {
        var shoppingCart = ShoppingCart.Create(shoppingCartDto.Id, shoppingCartDto.UserName);

        foreach (var item in shoppingCartDto.Items)
        {
            shoppingCart.AddItem(
                item.ProductId,
                item.Quantity,
                item.Color,
                item.Price,
                item.ProductName);
        }

        return shoppingCart;
    }
}
