using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Commands.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.CommandHandlers;

public class ErrorHandlingDecorator<T>(
    IHandleCommand<T> commandHandler,
    ILogger<ErrorHandlingDecorator<T>> logger)
    : CommandHandlerDecorator<T>(commandHandler)
    where T : ICommand
{
    public override async Task HandleAsync(T command)
    {
        try
        {
            await DecoratedHandler.HandleAsync((dynamic)command);
        }
        catch (Exception exception)
        {
            Type commandType = typeof(T);
            string commandTypeName = commandType.Name;

            logger.LogError(exception, "Command {Command} processing failed.", commandTypeName);
            throw;
        }
    }
}