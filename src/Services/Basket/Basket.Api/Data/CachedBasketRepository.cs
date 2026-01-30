using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        var basket = await repository.GetBasket(userName, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);

        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }
    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(userName, cancellationToken);
        
        await cache.RemoveAsync(userName, cancellationToken);

        return true;
    }
    /// <summary>
    /// Checks if a basket exists for the given user name.
    /// </summary>
    /// <param name="userName">user name</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> CheckBasket(string userName, CancellationToken cancellationToken = default)
    {
        var existsInCache = await cache.GetStringAsync(userName, cancellationToken);
        return !string.IsNullOrEmpty(existsInCache);
    }
}
