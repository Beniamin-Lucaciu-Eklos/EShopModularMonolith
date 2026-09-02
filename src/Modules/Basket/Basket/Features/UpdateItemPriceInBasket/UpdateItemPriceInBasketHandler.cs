
namespace EShop.Basket.Basket.Features.UpdateItemPriceInBasket;

public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price)
    : ICommand<UpdateItemPriceInBasketResult>;

public record UpdateItemPriceInBasketResult(bool IsSuccess);

public class UpdateItemPriceInBasketValidator: AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPriceInBasketValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty()
            .WithMessage("ProductId must not be empty.");

        RuleFor(x => x.Price).GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }
}

public class UpdateItemPriceInBasketHandler(BasketDbContext dbContext) :
    ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
{
    public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand command, CancellationToken cancellationToken)
    {
        var itemsToUpdate = await dbContext.ShoppingCartItems
            .Where(item => item.ProductId == command.ProductId)
            .ToListAsync(cancellationToken);

        if(!itemsToUpdate.Any())
            return new UpdateItemPriceInBasketResult(false);

        foreach (var item in itemsToUpdate)
        {
            item.UpdatePrice(command.Price);
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateItemPriceInBasketResult(true);
    }
}
