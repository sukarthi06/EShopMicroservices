using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlock.CQRS;

public sealed class QueryExecutor(IServiceProvider provider) : IQueryExecutor
{
    public Task<TResponse> ExecuteAsync<TQuery, TResponse>(
        TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
        var handler = provider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
        return handler.HandleAsync(query, cancellationToken);
    }
}

