namespace Enterprise.ApplicationServices.Commands.Model;

/// <summary>
/// This is a shared marker interface for all flavors of command objects.
/// Since we support both strict and pragmatic command interfaces, this can be used as a common ancestor or parent reference.
/// </summary>
public interface IBaseCommand;