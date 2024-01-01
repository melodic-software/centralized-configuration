using Enterprise.Events.Model;
using Enterprise.Events.Services.Handling;
using Enterprise.Events.Services.Handling.Generic;
using Enterprise.MediatR.Adapters.Factory.Abstract;
using System.Linq.Expressions;
using System.Reflection;

namespace Enterprise.MediatR.Adapters.Factory;

/// <summary>
/// Factory class responsible for creating instances of <see cref="EventHandlerAdapter{TEvent}"/>.
/// It compiles a lambda expression to construct the adapter, improving performance over
/// using reflection at each instantiation.
/// </summary>
/// <typeparam name="TEvent">The type of event that the adapter handles.</typeparam>
public class EventHandlerAdapterFactory<TEvent> : IEventHandlerAdapterFactory where TEvent : IEvent
{
    private readonly Func<IHandleEvent<TEvent>, object> _createAdapter;

    public EventHandlerAdapterFactory()
    {
        Type handlerType = typeof(IHandleEvent<TEvent>);
        Type adapterType = typeof(EventHandlerAdapter<TEvent>);
        ConstructorInfo? ctor = adapterType.GetConstructor([handlerType]);

        if (ctor == null)
            throw new InvalidOperationException($"No constructor found on {adapterType} that takes an argument of {handlerType}.");

        // Use compiled lambda expressions.
        ParameterExpression param = Expression.Parameter(handlerType, "handler");
        NewExpression body = Expression.New(ctor, param);
        Expression<Func<IHandleEvent<TEvent>, object>> lambda = Expression.Lambda<Func<IHandleEvent<TEvent>, object>>(body, param);

        _createAdapter = lambda.Compile();
    }

    public object CreateAdapter(IHandleEvent handler)
    {
        if (handler is IHandleEvent<TEvent> typedHandler)
            return _createAdapter(typedHandler);

        throw new ArgumentException($"Handler must be of type IHandleEvent<{typeof(TEvent).Name}>", nameof(handler));
    }
}