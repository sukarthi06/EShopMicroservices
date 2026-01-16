namespace Basket.Api.Basket.GetBasket;

public record GetBasketResult(ShoppingCart Cart);

// Ensure GetBasketQuery implements the correct interfaces
public record GetBasketQuery(string UserName) : IRequest<GetBasketResult>;

public class GetBasketQueryHandler(IBasketRepository repository) 
    : IRequestHandler<GetBasketQuery, GetBasketResult>
{
    public async ValueTask<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(query.UserName, cancellationToken);
        return new GetBasketResult(basket);
    }    
}
