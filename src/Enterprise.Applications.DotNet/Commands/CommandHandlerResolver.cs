using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Commands.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Applications.DotNet.Commands;

public class CommandHandlerResolver(IServiceProvider serviceProvider) : IResolveCommandHandler
{
    public IHandleCommand<T> GetHandlerFor<T>(T command) where T : ICommand
    {
        IHandleCommand<T> commandHandler = serviceProvider.GetRequiredService<CommandHandler<T>>();

        return commandHandler;
    }
}