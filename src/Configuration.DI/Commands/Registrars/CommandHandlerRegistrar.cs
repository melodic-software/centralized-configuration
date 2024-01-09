using Configuration.ApplicationServices.Applications.CreateApplication;
using Configuration.ApplicationServices.Applications.DeleteApplication;
using Configuration.ApplicationServices.Applications.UpdateApplication;
using Configuration.Domain.Applications;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.DI;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Enterprise.Events.Services.Raising;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Commands.Registrars;

internal static class CommandHandlerRegistrar
{
    internal static void RegisterCommandHandlers(IServiceCollection services)
    {
        services.RegisterCommandHandler(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();

            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new CreateApplicationHandler(
                eventRaiser,
                eventCallbackService,
                validationService,
                applicationExistenceService,
                applicationRepository
            ); ;
        });

        services.RegisterCommandHandler(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();

            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new UpdateApplicationHandler(
                eventRaiser,
                eventCallbackService,
                applicationExistenceService,
                validationService,
                applicationRepository
            );
        });

        services.RegisterCommandHandler(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();

            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new DeleteApplicationHandler(
                eventRaiser,
                eventCallbackService,
                applicationRepository
            );
        });
    }
}