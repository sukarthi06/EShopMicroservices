using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.Api.Basket.CheckoutBasket;

public record CheckoutBasketResult(bool IsSuccess);
public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto)
    : IRequest<CheckoutBasketResult>;//eShop.BuildingBlocks.CQRS.ICommand<CheckoutBasketResult>;

public class CheckoutBasketCommandValidator
    : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public sealed class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint)
    : IRequestHandler<CheckoutBasketCommand, CheckoutBasketResult>//eShop.BuildingBlocks.CQRS.ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async ValueTask<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        // get existing basket with total price
        // Set totalprice on basketcheckout event message
        // send basket checkout event to rabbitmq using masstransit
        // delete the basket

        var basket = await repository.GetBasket(command.BasketCheckoutDto.UserName, cancellationToken);
        if (basket == null)
        {
            return new CheckoutBasketResult(false);
        }

        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        await repository.DeleteBasket(command.BasketCheckoutDto.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}
