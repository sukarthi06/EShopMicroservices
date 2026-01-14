using BuildingBlock.CQRS;
using Microsoft.Extensions.Logging;

namespace BuildingBlock.Behaviors;

public class QueryLoggingDecorator<TQuery, TResponse>(
    IQueryHandler<TQuery, TResponse> inner,
    ILogger<QueryLoggingDecorator<TQuery, TResponse>> logger)
    : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> HandleAsync(
        TQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling query {Query}", typeof(TQuery).Name);
        return await inner.HandleAsync(query, cancellationToken);
    }
}


