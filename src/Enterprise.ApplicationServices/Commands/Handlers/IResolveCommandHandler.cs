using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Commands.Model;

namespace Enterprise.ApplicationServices.Commands.Handlers;

public interface IResolveCommandHandler
{
    IHandleCommand<T> GetHandlerFor<T>(T command) where T : ICommand;
}