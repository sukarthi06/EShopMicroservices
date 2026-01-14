using BuildingBlock.CQRS;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlock.Behaviors;

public class LoggingDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
    private readonly ICommandHandler<TCommand, TResponse> _inner;
    private readonly ILogger<LoggingDecorator<TCommand, TResponse>> _logger;

    public LoggingDecorator(ICommandHandler<TCommand, TResponse> inner, ILogger<LoggingDecorator<TCommand, TResponse>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling {Command}", typeof(TCommand).Name);
        var sw = Stopwatch.StartNew();

        var response = await _inner.HandleAsync(command, cancellationToken);

        sw.Stop();
        if (sw.Elapsed.TotalSeconds > 3)
        {
            _logger.LogWarning("Long Running Command {Command} took {Elapsed}s", typeof(TCommand).Name, sw.Elapsed.TotalSeconds);
        }

        _logger.LogInformation("Handled {Command}", typeof(TCommand).Name);

        return response;
    }
}
