namespace BuildingBlock.CQRS;

public interface ICommandExecutor
{
    Task<TResponse> ExecuteAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>
        where TResponse : notnull;
}

