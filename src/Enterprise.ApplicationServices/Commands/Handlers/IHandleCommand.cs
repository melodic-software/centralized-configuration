﻿using Enterprise.ApplicationServices.Commands.Model;

namespace Enterprise.ApplicationServices.Commands.Handlers;

/// <summary>
/// Handle the command.
/// </summary>
public interface IHandleCommand
{
    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task HandleAsync(ICommand command);
}

/// <summary>
/// Handle the command.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IHandleCommand<in T> : IApplicationService where T : ICommand
{
    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task HandleAsync(T command);
}

/// <summary>
/// According to Bertrand Meyer, commands return void (there is no return value).
/// Typically, the client of the command handler needs to know if the operation succeeded or failed.
/// This is a pragmatic interface that allows a return value.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IHandleCommand<in TCommand, TResult> : IApplicationService where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<TResult> HandleAsync(TCommand command);
}