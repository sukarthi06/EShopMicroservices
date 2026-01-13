using BuildingBlock.CQRS;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlock.Behaviors;

public sealed class LoggingQueryDecorator<TQuery, TResponse>
    : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull
{
    private readonly IQueryHandler<TQuery, TResponse> _inner;
    private readonly ILogger<LoggingQueryDecorator<TQuery, TResponse>> _logger;

    public LoggingQueryDecorator(
        IQueryHandler<TQuery, TResponse> inner,
        ILogger<LoggingQueryDecorator<TQuery, TResponse>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<TResponse> HandleAsync(TQuery query)
    {
        _logger.LogInformation(
            "[START] Handle query={Query} - Response={Response} - QueryData={QueryData}",
            typeof(TQuery).Name,
            typeof(TResponse).Name,
            query);

        var timer = Stopwatch.StartNew();

        var response = await _inner.HandleAsync(query);

        timer.Stop();

        if (timer.Elapsed.Seconds > 3)
        {
            _logger.LogWarning(
                "[PERFORMANCE] The query {Query} took {TimeTaken} seconds.",
                typeof(TQuery).Name,
                timer.Elapsed.Seconds);
        }

        _logger.LogInformation(
            "[END] Handled {Query} with {Response}",
            typeof(TQuery).Name,
            typeof(TResponse).Name);

        return response;
    }
}
