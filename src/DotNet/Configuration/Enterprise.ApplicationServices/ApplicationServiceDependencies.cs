using Enterprise.ApplicationServices.Abstractions;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;

namespace Enterprise.ApplicationServices;

public class ApplicationServiceDependencies : IApplicationServiceDependencies
{
    public IRaiseEvents EventRaiser { get; }
    public IEventCallbackService EventCallbackService { get; }

    public ApplicationServiceDependencies(IRaiseEvents eventRaiser, IEventCallbackService eventCallbackService)
    {
        EventRaiser = eventRaiser ?? throw new ArgumentNullException(nameof(eventRaiser));
        EventCallbackService = eventCallbackService ?? throw new ArgumentNullException(nameof(eventCallbackService));
    }
}