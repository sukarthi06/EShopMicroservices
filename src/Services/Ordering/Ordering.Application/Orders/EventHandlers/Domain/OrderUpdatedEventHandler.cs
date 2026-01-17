
namespace Ordering.Application.Orders.EventHandlers;

public sealed class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger) 
    : INotificationHandler<OrderUpdatedEvent>
{
    public ValueTask Handle(OrderUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
        return ValueTask.CompletedTask;
    }
}
