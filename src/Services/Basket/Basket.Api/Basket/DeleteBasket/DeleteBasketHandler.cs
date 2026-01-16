namespace Basket.Api.Basket.DeleteBasket;

public record DeleteBasketResult(bool IsSuccess);
public record DeleteBasketCommand(string UserName) : IRequest<DeleteBasketResult>;

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public sealed class DeleteBasketCommandHandler(IBasketRepository repository) 
    : IRequestHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async ValueTask<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        await repository.DeleteBasket(command.UserName, cancellationToken);
        return new DeleteBasketResult(IsSuccess: true);
    }
}
