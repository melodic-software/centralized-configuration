using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Model;

namespace Enterprise.ApplicationServices.Commands.Handlers.Generic;

public interface IHandleCommand<in T> : IApplicationService where T : ICommand 
{
    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task HandleAsync(T command);
}