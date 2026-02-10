using System.Diagnostics;
using Mediator;
using Microsoft.Extensions.Logging;

namespace eShop.BuildingBlocks.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    public async ValueTask<TResponse> Handle(
        TRequest request,
        MessageHandlerDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name,
            typeof(TResponse).Name,
            request);

        var timer = Stopwatch.StartNew();

        var response = await next(request, cancellationToken);

        timer.Stop();

        if (timer.Elapsed.TotalSeconds > 3)
        {
            logger.LogWarning(
                "[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
                typeof(TRequest).Name,
                timer.Elapsed.TotalSeconds);
        }

        logger.LogInformation(
            "[END] Handled {Request} with {Response}",
            typeof(TRequest).Name,
            typeof(TResponse).Name);

        return response;
    }
}
