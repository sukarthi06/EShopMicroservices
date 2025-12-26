using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler
    (/*IPublishEndpoint publishEndpoint,*/ IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger) 
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        //if(await featureManager.IsEnabledAsync("OrderFullfillment"))
        //{
        //    var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();
        //    await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        //}        
        return Task.CompletedTask;
    }
}
