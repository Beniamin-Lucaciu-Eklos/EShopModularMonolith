namespace EShop.Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string UserName)
    : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCart);

public class GetBasketHandler(IBasketRepository basketRepository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var shoppingCart = await basketRepository.GetBasket(query.UserName, cancellationToken: cancellationToken);

        var shoppingCartDto = shoppingCart.Adapt<ShoppingCartDto>();
        return new GetBasketResult(shoppingCartDto);
    }
}
