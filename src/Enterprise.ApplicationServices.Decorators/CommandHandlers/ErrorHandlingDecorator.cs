using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.Commands.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.CommandHandlers;

public class ErrorHandlingDecorator<T> : CommandHandlerDecorator<T>
    where T : ICommand
{
    private readonly ILogger<ErrorHandlingDecorator<T>> _logger;

    public ErrorHandlingDecorator(IHandleCommand<T> commandHandler,
        ILogger<ErrorHandlingDecorator<T>> logger) : base(commandHandler)
    {
        _logger = logger;
    }

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

            _logger.LogError(exception, "Command {Command} processing failed.", commandTypeName);
            throw;
        }
    }
}