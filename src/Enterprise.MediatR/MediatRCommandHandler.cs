using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.MediatR.Adapters;
using MediatR;

namespace Enterprise.MediatR;

public class MediatRCommandHandler<T>(
    IApplicationServiceDependencies appServiceDependencies,
    IMediator mediator)
    : CommandHandler<T>(appServiceDependencies)
    where T : ICommand
{
    public override async Task HandleAsync(T command)
    {
        IRequest request = new CommandAdapter<T>(command);
        await mediator.Send(request, CancellationToken.None);
    }
}