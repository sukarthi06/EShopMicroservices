namespace Ordering.Application.Orders.Commands.UpdateOrder;

public sealed record UpdateOrderResult(bool IsSuccess);
public sealed record UpdateOrderCommand(OrderDto Order)
    : IRequest<UpdateOrderResult>;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required");
    }
}