namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public sealed record DeleteOrderResult(bool IsSuccess);

    public sealed record DeleteOrderCommand(Guid OrderId)
    : IRequest<DeleteOrderResult>;

    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId is required");
        }
    }
}
