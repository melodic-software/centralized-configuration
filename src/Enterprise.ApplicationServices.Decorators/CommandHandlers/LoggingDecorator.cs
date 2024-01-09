using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.Commands.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.CommandHandlers;

public class LoggingDecorator<T> : CommandHandlerDecorator<T>
    where T : ICommand
{
    private readonly ILogger<LoggingDecorator<T>> _logger;

    public LoggingDecorator(IHandleCommand<T> commandHandler,
        ILogger<LoggingDecorator<T>> logger) : base(commandHandler)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(T command)
    {
        Type commandType = typeof(T);
        string commandTypeName = commandType.Name;

        _logger.LogInformation("Executing command {Command}.", commandTypeName);
        await DecoratedHandler.HandleAsync((dynamic)command);
        _logger.LogInformation("Command {Command} processed successfully.", commandTypeName);
    }
}