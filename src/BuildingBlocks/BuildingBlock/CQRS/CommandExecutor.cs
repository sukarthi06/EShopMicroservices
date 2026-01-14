using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlock.CQRS;

public class CommandExecutor : ICommandExecutor
{
    private readonly IServiceProvider _provider;

    public CommandExecutor(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<TResponse> ExecuteAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>
        where TResponse : notnull
    {
        var handler = _provider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
        return handler.HandleAsync(command, cancellationToken);
    }
}

