using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Model;

namespace Enterprise.ApplicationServices.Commands.Handlers.Generic;

public abstract class CommandHandler<T>(IApplicationServiceDependencies appServiceDependencies)
    : ApplicationService(appServiceDependencies), IHandleCommand<T> where T : ICommand
{
    public async Task HandleAsync(ICommand command)
    {
        Validate(command);

        // this is a dynamic dispatch
        await HandleAsync((dynamic)command);
    }

    public abstract Task HandleAsync(T command);

    private void Validate(ICommand command)
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