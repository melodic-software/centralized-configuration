using MediatR;
using Enterprise.ApplicationServices.Commands.Model;

namespace Enterprise.MediatR.Adapters;

public class CommandAdapter<TCommand>(TCommand command) : IRequest
    where TCommand : ICommand
{
    public TCommand Command { get; } = command;
}