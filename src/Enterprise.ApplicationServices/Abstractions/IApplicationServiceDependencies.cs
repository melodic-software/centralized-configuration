using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;

namespace Enterprise.ApplicationServices.Abstractions;

/// <summary>
/// There are several services that are required by all application service instances.
/// This abstraction was created to house all of those in one object.
/// </summary>
public interface IApplicationServiceDependencies
{
    public IRaiseEvents EventRaiser { get; }
    public IEventCallbackService EventCallbackService { get; }
}