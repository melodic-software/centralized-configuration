using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.ApplicationServices.Decorators.CommandHandlers;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;
using Enterprise.ApplicationServices.Queries.Model;
using Enterprise.DI.DotNet.Extensions;
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
}