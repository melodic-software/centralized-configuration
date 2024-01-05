using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Commands.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.CommandHandlers;

public class LoggingDecorator<T>(
    IHandleCommand<T> commandHandler,
    ILogger<LoggingDecorator<T>> logger)
    : CommandHandlerDecorator<T>(commandHandler)
    where T : ICommand
{
    public override async Task HandleAsync(T command)
    {
        Type commandType = typeof(T);
        string commandTypeName = commandType.Name;

        logger.LogInformation("Executing command {Command}.", commandTypeName);

        try
        {
            await DecoratedHandler.HandleAsync((dynamic)command);

            logger.LogInformation("Command {Command} processed successfully.", commandTypeName);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Command {Command} processing failed.", commandTypeName);
            throw;
        }
    }
}