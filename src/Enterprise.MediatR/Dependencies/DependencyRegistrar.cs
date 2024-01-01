using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;
using Enterprise.ApplicationServices.Queries.Model;
using Enterprise.Domain.Generic;
using Enterprise.Events.Services.Handling;
using Enterprise.Events.Services.Handling.Generic;
using Enterprise.Events.Services.Raising;
using Enterprise.MediatR.Adapters;
using Enterprise.MediatR.Adapters.Factory;
using Enterprise.MediatR.Adapters.Factory.Abstract;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.MediatR.Dependencies;

public static class DependencyRegistrar
{
    public static void RegisterCommandHandler<T>(this IServiceCollection services,
        Func<IServiceProvider, IHandleCommand<T>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where T : ICommand
    {
        services.Add(new ServiceDescriptor(typeof(IRequestHandler<CommandAdapter<T>>), provider =>
        {
            IHandleCommand<T> commandHandler = factory(provider);
            IRequestHandler<CommandAdapter<T>> requestHandler = new CommandHandlerAdapter<T>(commandHandler);
            return requestHandler;
        }, serviceLifetime));

        services.Add(new ServiceDescriptor(typeof(IHandleCommand<T>), provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IMediator mediator = provider.GetRequiredService<IMediator>();
            IHandleCommand<T> commandHandler = new MediatRCommandHandler<T>(appServiceDependencies, mediator);
            return commandHandler;
        }, serviceLifetime));
    }

    public static void RegisterQueryHandler<TQuery, TResult>(this IServiceCollection services,
        Func<IServiceProvider, IHandleQuery<TQuery, TResult>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TQuery : IQuery
    {
        services.Add(new ServiceDescriptor(typeof(IRequestHandler<QueryAdapter<TQuery, TResult>, Result<TResult>>), provider =>
        {
            IHandleQuery<TQuery, TResult> queryHandler = factory(provider);
            IRequestHandler<QueryAdapter<TQuery, TResult>, Result<TResult>> requestHandler = new QueryHandlerAdapter<TQuery, TResult>(queryHandler);
            return requestHandler;
        }, serviceLifetime));

        services.Add(new ServiceDescriptor(typeof(IHandleQuery<TQuery, TResult>), provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IMediator mediator = provider.GetRequiredService<IMediator>();
            MediatRQueryHandler<TQuery, TResult> queryHandler = new MediatRQueryHandler<TQuery, TResult>(appServiceDependencies, mediator);
            return queryHandler;
        }, serviceLifetime));
    }

    /// <summary>
    /// Registers an event handler within the service collection for dependency injection.
    /// This method automatically discovers all event types handled by the given handler
    /// and sets up the necessary MediatR notification handlers.
    /// </summary>
    /// <typeparam name="THandler">The type of the event handler.</typeparam>
    /// <param name="services">The service collection to which the event handler will be added.</param>
    /// <param name="factory">A factory method to create instances of the event handler.</param>
    public static void RegisterEventHandler<THandler>(this IServiceCollection services, Func<IServiceProvider, THandler> factory)
        where THandler : class, IHandleEvent
    {
        List<Type> handlerInterfaces = typeof(THandler).GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandleEvent<>))
            .ToList();

        if (!handlerInterfaces.Any())
            throw new InvalidOperationException("The specified type does not implement IHandleEvent<T> for any T.");

        foreach (Type handlerInterface in handlerInterfaces)
        {
            Type eventType = handlerInterface.GetGenericArguments()[0];
            Type eventHandlerType = typeof(INotificationHandler<>).MakeGenericType(typeof(EventAdapter<>).MakeGenericType(eventType));
            Type adapterFactoryType = typeof(EventHandlerAdapterFactory<>).MakeGenericType(eventType);

            // Register the adapter factory for each specific TEvent type.
            services.AddSingleton(adapterFactoryType,
                Activator.CreateInstance(adapterFactoryType)
                ?? throw new InvalidOperationException($"Failed to create an instance of {adapterFactoryType}."));

            // Register the adapter in the MediatR pipeline.
            services.Add(new ServiceDescriptor(eventHandlerType, provider =>
            {
                THandler handlerInstance = factory(provider);
                // Resolve the specific adapter factory for the TEvent type.
                IEventHandlerAdapterFactory adapterFactory = (IEventHandlerAdapterFactory)provider.GetRequiredService(adapterFactoryType);
                return adapterFactory.CreateAdapter(handlerInstance);
            }, ServiceLifetime.Transient));
        }

        // Register THandler implementation
        services.Add(ServiceDescriptor.Describe(typeof(THandler), factory, ServiceLifetime.Transient));
    }

    public static void RegisterEventRaiser(this IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            IMediator mediator = provider.GetRequiredService<IMediator>();
            IRaiseEvents eventRaiser = new MediatREventRaiser(mediator);
            return eventRaiser;
        });
    }
}