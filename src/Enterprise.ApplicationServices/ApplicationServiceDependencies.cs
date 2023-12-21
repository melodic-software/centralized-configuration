using Enterprise.ApplicationServices.Abstractions;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;

namespace Enterprise.ApplicationServices;

public class ApplicationServiceDependencies(IRaiseEvents eventRaiser, IEventCallbackService eventCallbackService)
    : IApplicationServiceDependencies
{
    public IRaiseEvents EventRaiser { get; } = eventRaiser ?? throw new ArgumentNullException(nameof(eventRaiser));
    public IEventCallbackService EventCallbackService { get; } = eventCallbackService ?? throw new ArgumentNullException(nameof(eventCallbackService));
}