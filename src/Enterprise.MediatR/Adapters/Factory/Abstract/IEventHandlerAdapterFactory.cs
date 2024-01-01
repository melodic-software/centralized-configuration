using Enterprise.Events.Services.Handling;
using MediatR;

namespace Enterprise.MediatR.Adapters.Factory.Abstract;

/// <summary>
/// Defines a factory for creating event handler adapters.
/// An implementation of this interface must be able to take a handler
/// and wrap it in an appropriate <see cref="INotificationHandler{TNotification}"/> for MediatR.
/// </summary>
public interface IEventHandlerAdapterFactory
{
    /// <summary>
    /// Creates an adapter for the given event handler.
    /// </summary>
    /// <param name="handler">The event handler to wrap.</param>
    /// <returns>An adapter that implements <see cref="INotificationHandler{TNotification}"/>.</returns>
    object CreateAdapter(IHandleEvent handler);
}