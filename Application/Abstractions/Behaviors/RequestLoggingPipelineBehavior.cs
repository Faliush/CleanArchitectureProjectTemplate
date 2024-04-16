using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Domain.Core.Primitives.Result;

namespace Application.Abstractions.Behaviors;

internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    private readonly ILogger _logger;

    public RequestLoggingPipelineBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        _logger.LogInformation(
            "Processing request {RequestName}", requestName);

        TResponse result = await next();

        if (result.Error is null)
        {
            _logger.LogInformation(
                "Completed request {RequestName}", requestName);
        }
        else
        {
            using (LogContext.PushProperty("Error", result.Error, true))
            {
                _logger.LogError(
                    "Completed request {RequestName} with error", requestName);
            }
        }

        return result;
    }
}
