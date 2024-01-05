using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.ApplicationServices.Decorators.CommandHandlers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.DI
{
    public static class DependencyRegistrar
    {
        public static void RegisterCommandHandler<T>(this IServiceCollection services,
            Func<IServiceProvider, IHandleCommand<T>> factory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where T : ICommand
        {
            services.Add(ServiceDescriptor.Describe(typeof(IHandleCommand<T>), provider =>
            {
                // Create the base instance
                IHandleCommand<T> commandHandler = factory(provider);

                // Decorate with error handling / logging.
                ILogger<LoggingDecorator<T>> logger = provider.GetRequiredService<ILogger<LoggingDecorator<T>>>();
                LoggingDecorator<T> loggingDecorator = new LoggingDecorator<T>(commandHandler, logger);

                // Decorate with command validation.
                IEnumerable<IValidator<T>> validators = provider.GetServices<IValidator<T>>();
                FluentValidationDecorator<T> validationDecorator = new FluentValidationDecorator<T>(loggingDecorator, validators);

                return validationDecorator;

            }, serviceLifetime));
        }
    }
}
