namespace Enterprise.ApplicationServices.Commands.Model.Generic;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a command object.
/// It is used primarily for constraint purposes.
/// According to Bertrand Meyer, commands don't have a return value. They return void.
/// This interface is a pragmatic compromise that allows for defining a result associated with the command.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IPragmaticCommand<TResponse> : IBaseCommand;