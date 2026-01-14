namespace BuildingBlock.CQRS;

public interface IQueryExecutor
{
    Task<TResponse> ExecuteAsync<TQuery, TResponse>(
        TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>
        where TResponse : notnull;
}

