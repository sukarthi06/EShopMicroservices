namespace BuildingBlock.CQRS;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull
{
    Task<TResponse> HandleAsync(TQuery query);
}

