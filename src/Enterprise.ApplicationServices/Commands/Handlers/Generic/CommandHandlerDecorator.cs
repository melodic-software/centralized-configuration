using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.Events.Model;

namespace Enterprise.ApplicationServices.Commands.Handlers.Generic
{
    public abstract class CommandHandlerDecorator<T>(IHandleCommand<T> commandHandler) : IHandleCommand<T>
        where T : ICommand
    {
        protected IHandleCommand<T> DecoratedHandler { get; } = commandHandler;

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
}
