using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.ApplicationServices.Events;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Commands.Handlers.Generic;

public abstract class CommandHandler<T> : ApplicationService, IHandleCommand<T> where T : ICommand
{
    private readonly ILogger<CommandHandler<T>> _logger;

    protected CommandHandler(
        IEventServiceFacade eventServiceFacade,
        ILogger<CommandHandler<T>> logger)
        : base(eventServiceFacade, logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task HandleAsync(ICommand command)
    {
        try
        {
            ValidateType(command);
            await HandleAsync((dynamic)command);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error handling command of type {command.GetType().Name}");
            throw;
        }
    }

    public abstract Task HandleAsync(T command);

    private void ValidateType(ICommand command)
    {
        Type genericArgumentType = typeof(T);
        Type commandType = command.GetType();

        bool commandCanBeHandled = commandType == genericArgumentType;

        if (commandCanBeHandled)
            return;

        Type commandHandlerType = GetType();

        throw new Exception(CommandCannotBeHandled(commandType, commandHandlerType));
    }

    private static string CommandCannotBeHandled(Type commandType, Type commandHandlerType) =>
        $"A command of type \"{commandType.FullName}\" cannot be handled by \"{commandHandlerType.FullName}\"";
}