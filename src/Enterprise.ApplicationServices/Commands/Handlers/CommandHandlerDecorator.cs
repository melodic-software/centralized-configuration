using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.Events.Model;

namespace Enterprise.ApplicationServices.Commands.Handlers;

public abstract class CommandHandlerDecorator<T> : IHandleCommand<T>
    where T : ICommand
{
    protected CommandHandlerDecorator(IHandleCommand<T> commandHandler)
    {
        DecoratedHandler = commandHandler;
    }

    protected IHandleCommand<T> DecoratedHandler { get; }

    public abstract Task HandleAsync(T command);
    
    public void ClearCallbacks()
    {
        DecoratedHandler.ClearCallbacks();
    }

    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
    {
        DecoratedHandler.RegisterEventCallback(eventCallback);
    }
}