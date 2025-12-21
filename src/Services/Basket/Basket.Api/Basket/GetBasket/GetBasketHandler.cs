
using Basket.Api.Data;

namespace Basket.Api.Basket.GetBasket;

public record GetBasketResult(ShoppingCart Cart);
public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

public class GetBasketQueryHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(query.UserName, cancellationToken);
        return new GetBasketResult(basket);
    }
}
