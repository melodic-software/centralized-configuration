namespace Enterprise.ApplicationServices.Commands.Model;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a command object.
/// It is used primarily for constraint purposes.
/// </summary>
public interface ICommand : IBaseCommand
{

}

/// <summary>
/// This is a marker interface that signifies that an implementing class is a command object.
/// It is used primarily for constraint purposes.
/// Typically, commands don't have a return value (return void),
/// but this is a pragmatic compromise that allows for defining a result.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<TResponse> : IBaseCommand
{

}

/// <summary>
/// This is a shared marker interface for all flavors of command objects.
/// Since we support both strict and pragmatic command interfaces, this can be used as a common ancestor or parent reference.
/// </summary>
public interface IBaseCommand
{

}