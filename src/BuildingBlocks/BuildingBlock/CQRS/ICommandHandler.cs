namespace BuildingBlock.CQRS;

public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
    Task<TResponse> HandleAsync(TCommand command);
}


