using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Commands.Model;
using MediatR;

namespace Enterprise.MediatR.Adapters;

public class CommandHandlerAdapter<T>(IHandleCommand<T> commandHandler)
    : IRequestHandler<CommandAdapter<T>> where T : ICommand
{
    public async Task Handle(CommandAdapter<T> request, CancellationToken cancellationToken)
    {
        await commandHandler.HandleAsync(request.Command);
    }
}