using Enterprise.ApplicationServices;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Registrars;

internal class ApplicationServiceRegistrar
{
    internal static void RegisterApplicationServiceDependencies(IServiceCollection services)
    {
        services.AddTransient(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();

            IApplicationServiceDependencies applicationServiceDependencies = new ApplicationServiceDependencies(eventRaiser, eventCallbackService);

            return applicationServiceDependencies;
        });
    }
}