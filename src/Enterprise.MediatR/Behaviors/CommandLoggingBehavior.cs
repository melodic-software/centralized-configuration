using Enterprise.MediatR.Adapters.Abstract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors;

public class CommandLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandAdapter
{
    private readonly ILogger<CommandLoggingBehavior<TRequest, TResponse>> _logger;

    public CommandLoggingBehavior(ILogger<CommandLoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string name = typeof(TRequest).Name;

        _logger.LogInformation("Executing command {Command}.", name);

        try
        {
            TResponse response = await next();

            _logger.LogInformation("Command {Command} processed successfully.", name);

            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Command {Command} processing failed.", name);
            throw;
        }
    }
}