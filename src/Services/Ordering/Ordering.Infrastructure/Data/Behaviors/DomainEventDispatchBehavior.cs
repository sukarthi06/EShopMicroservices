using Mediator;
using Ordering.Application.Data;

namespace Ordering.Infrastructure.Data.Behaviors;

public sealed class DomainEventDispatchBehavior<TMessage, TResponse>(
    IMediator mediator,
    IApplicationDbContext dbContext)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next(message, cancellationToken);

        var entities = dbContext.GetAggregatesWithDomainEvents();

        foreach (var entity in entities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();

            foreach (var domainEvent in events)
                await mediator.Publish(domainEvent, cancellationToken);
        }

        return response;
    }
}
