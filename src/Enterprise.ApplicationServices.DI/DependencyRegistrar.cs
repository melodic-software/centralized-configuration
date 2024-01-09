using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.ApplicationServices.Decorators.CommandHandlers;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.ApplicationServices.Queries.Model;
using Enterprise.DI.DotNet.Extensions;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.DI;

public static class DependencyRegistrar
{
    public static void RegisterCommandHandler<T>(this IServiceCollection services,
        Func<IServiceProvider, IHandleCommand<T>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where T : ICommand
    {
        services.BeginRegistration<IHandleCommand<T>>()
            .Add(factory, serviceLifetime)
            .WithDecorators((provider, commandHandler) =>
            {
                IEnumerable<IValidator<T>> validators = provider.GetServices<IValidator<T>>();
                FluentValidationDecorator<T> decorator = new FluentValidationDecorator<T>(commandHandler, validators);
                return decorator;
            }, (provider, commandHandler) =>
            {
                ILogger<LoggingDecorator<T>> logger = provider.GetRequiredService<ILogger<LoggingDecorator<T>>>();
                LoggingDecorator<T> decorator = new LoggingDecorator<T>(commandHandler, logger);
                return decorator;
            }, (provider, commandHandler) =>
            {
                ILogger<ErrorHandlingDecorator<T>> logger = provider.GetRequiredService<ILogger<ErrorHandlingDecorator<T>>>();
                ErrorHandlingDecorator<T> decorator = new ErrorHandlingDecorator<T>(commandHandler, logger);
                return decorator;
            });
    }

    public static void RegisterQueryHandler<TQuery, TResult>(this IServiceCollection services,
        Func<IServiceProvider, IHandleQuery<TQuery, TResult>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TQuery : IQuery
    {
        services.BeginRegistration<IHandleQuery<TQuery, TResult>>()
            .Add(factory, serviceLifetime);
    }

    public static void RegisterSimpleQueryHandler<TQuery, TResult>(this IServiceCollection services,
        Func<IServiceProvider, IQueryLogic<TQuery, TResult>> queryLogicFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TQuery : IQuery
    {
        services.BeginRegistration<IHandleQuery<TQuery, TResult>>()
            .Add(provider =>
            {
                IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
                IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();
                ILogger<SimpleQueryHandler<TQuery, TResult>> logger = provider.GetRequiredService<ILogger<SimpleQueryHandler<TQuery, TResult>>>();

                // Resolve the query logic implementation.
                IQueryLogic<TQuery, TResult> queryLogic = queryLogicFactory(provider);

                // Use a common handler that delegates to the query logic.
                // We can still add cross-cutting concerns and decorate this handler as needed.
                IHandleQuery<TQuery, TResult> queryHandler = new SimpleQueryHandler<TQuery, TResult>(eventRaiser, eventCallbackService, logger, queryLogic);

                return queryHandler;
            }, serviceLifetime);
    }
}