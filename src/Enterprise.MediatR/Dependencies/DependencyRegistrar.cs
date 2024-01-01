using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.MediatR.Adapters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            IApplicationServiceDependencies appServiceDependencies =
                provider.GetRequiredService<IApplicationServiceDependencies>();
            IMediator mediator = provider.GetRequiredService<IMediator>();
            IHandleCommand<T> commandHandler = new MediatRCommandHandler<T>(appServiceDependencies, mediator);
            return commandHandler;
        }, serviceLifetime));
    }
}