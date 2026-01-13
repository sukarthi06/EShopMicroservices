using BuildingBlock.CQRS;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlock.Behaviors;

//builder.Services.Decorate(
//    typeof(ICommandHandler<,>),
//    typeof(LoggingDecorator<,>)
//);

public class LoggingDecorator<TCommand, TResponse>
    : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
    private readonly ICommandHandler<TCommand, TResponse> _inner;
    private readonly ILogger<LoggingDecorator<TCommand, TResponse>> _logger;

    public LoggingDecorator(
        ICommandHandler<TCommand, TResponse> inner,
        ILogger<LoggingDecorator<TCommand, TResponse>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<TResponse> HandleAsync(TCommand command)
    {
        _logger.LogInformation(
            "[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TCommand).Name,
            typeof(TResponse).Name,
            command);

        var timer = Stopwatch.StartNew();

        var response = await _inner.HandleAsync(command);

        timer.Stop();

        if (timer.Elapsed.Seconds > 3)
        {
            _logger.LogWarning(
                "[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
                typeof(TCommand).Name,
                timer.Elapsed.Seconds);
        }

        _logger.LogInformation(
            "[END] Handled {Request} with {Response}",
            typeof(TCommand).Name,
            typeof(TResponse).Name);

        return response;
    }
}

