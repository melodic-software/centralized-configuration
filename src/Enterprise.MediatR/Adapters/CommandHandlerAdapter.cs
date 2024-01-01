using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.Commands.Model;
using MediatR;

namespace Enterprise.MediatR.Adapters;

public class CommandHandlerAdapter<T>(
    IApplicationServiceDependencies applicationServiceDependencies,
    IMediator mediator)
    : CommandHandler<T>(applicationServiceDependencies)
    where T : ICommand
{
    public override async Task HandleAsync(T command)
    {
        CommandAdapter<T> adaptedCommand = new CommandAdapter<T>(command);
        await mediator.Send(adaptedCommand);
    }
}