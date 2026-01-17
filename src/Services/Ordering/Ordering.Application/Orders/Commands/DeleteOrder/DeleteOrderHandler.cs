namespace Ordering.Application.Orders.Commands.DeleteOrder;

public sealed class DeleteOrderHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async ValueTask<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.OrderId);
        var order = await dbContext.Orders
            .FindAsync([orderId], cancellationToken: cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }

        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteOrderResult(true);
    }
}
