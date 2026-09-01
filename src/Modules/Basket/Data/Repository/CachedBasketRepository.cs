namespace EShop.Basket.Data.Repository;

public class CachedBasketRepository(
    IBasketRepository basketRepository,
    IDistributedCache cache)
    : IBasketRepository
{

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        Converters = {
            new ShoppingCartJsonConverter(),
            new ShoppingCartItemJsonConverter()
        }
    };

    public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (!asNoTracking)
        {
            return await basketRepository.GetBasket(userName, asNoTracking, cancellationToken);
        }

        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrWhiteSpace(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket, _options)!;
        }

        var basket = await basketRepository.GetBasket(userName, asNoTracking, cancellationToken);

        await cache.SetStringAsync(userName, SerializeAsJson(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await basketRepository.CreateBasket(basket, cancellationToken);

        await cache.SetStringAsync(basket.UserName, SerializeAsJson(basket), cancellationToken);

        return basket;
    }

    private string SerializeAsJson(ShoppingCart basket)
    {
        return JsonSerializer.Serialize(basket, _options);
    }

    public async Task<bool> DeleteBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        await basketRepository.DeleteBasket(userName, asNoTracking, cancellationToken);

        await cache.RemoveAsync(userName, cancellationToken);

        return true;
    }

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        var result = await basketRepository.SaveChangesAsync(userName, cancellationToken);

        if (userName is not null)
        {
            await cache.RemoveAsync(userName, cancellationToken);
        }

        return result;
    }
}
