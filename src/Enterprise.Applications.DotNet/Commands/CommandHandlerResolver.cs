﻿using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.Commands.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Applications.DotNet.Commands;

public class CommandHandlerResolver : IResolveCommandHandler
{
    private readonly IServiceProvider _serviceProvider;

    public CommandHandlerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IHandleCommand<T> GetHandlerFor<T>(T command) where T : ICommand
    {
        IHandleCommand<T> commandHandler = _serviceProvider.GetRequiredService<CommandHandlerBase<T>>();

        return commandHandler;
    }
}