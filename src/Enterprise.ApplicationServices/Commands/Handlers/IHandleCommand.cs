using Enterprise.ApplicationServices.Commands.Model;

namespace Enterprise.ApplicationServices.Commands.Handlers;

public interface IHandleCommand
{
    public Task HandleAsync(ICommand command);
}