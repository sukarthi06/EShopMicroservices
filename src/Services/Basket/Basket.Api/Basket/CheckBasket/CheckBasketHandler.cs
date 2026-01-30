namespace Basket.Api.Basket.CheckBasket;

public record CheckBasketResult(bool IsSuccess);
public record CheckBasketQuery(string UserName)
    : IRequest<CheckBasketResult>;

public class CheckBasketQueryValidator
    : AbstractValidator<CheckBasketQuery>
{
    public CheckBasketQueryValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public sealed class CheckBasketQueryHandler(IBasketRepository repository) : IRequestHandler<CheckBasketQuery, CheckBasketResult>
{
    public async ValueTask<CheckBasketResult> Handle(CheckBasketQuery query, CancellationToken cancellationToken)
    {
        var result = await repository.CheckBasket(query.UserName, cancellationToken);
        return new CheckBasketResult(result);
    }
}
