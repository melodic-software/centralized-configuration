using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.MediatR.Adapters.Abstract;

namespace Enterprise.MediatR.Adapters;

public class CommandAdapter<TCommand>(TCommand command)
    : ICommandAdapter where TCommand : ICommand
{
    public TCommand Command { get; } = command;
}