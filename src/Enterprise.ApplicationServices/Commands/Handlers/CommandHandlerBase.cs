using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.ApplicationServices.Events;

namespace Enterprise.ApplicationServices.Commands.Handlers;

public abstract class CommandHandlerBase<T> : ApplicationServiceBase, IHandleCommand<T> where T : ICommand
{
    protected CommandHandlerBase(IEventServiceFacade eventServiceFacade) : base(eventServiceFacade)
    {

    }

    public async Task HandleAsync(ICommand command)
    {
        ValidateType(command);
        await HandleAsync((dynamic)command);
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