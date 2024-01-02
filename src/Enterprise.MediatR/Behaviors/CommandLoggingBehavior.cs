using Enterprise.MediatR.Adapters.Abstract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors;

public class CommandLoggingBehavior<TRequest, TResponse>(ILogger<CommandLoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandAdapter
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string name = typeof(TRequest).Name;

        logger.LogWarning("Executing command {Command}", name);

        try
        {
            TResponse response = await next();

            logger.LogWarning("Command {Command} processed successfully", name);

            return response;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Command {Command} processing failed", name);
            throw;
        }
    }
}