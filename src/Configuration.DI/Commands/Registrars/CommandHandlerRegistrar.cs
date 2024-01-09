using Configuration.ApplicationServices.Applications.CreateApplication;
using Configuration.ApplicationServices.Applications.DeleteApplication;
using Configuration.ApplicationServices.Applications.UpdateApplication;
using Configuration.Domain.Applications;
using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.DI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Enterprise.ApplicationServices.Events;

namespace Configuration.DI.Commands.Registrars;

internal static class CommandHandlerRegistrar
{
    internal static void RegisterCommandHandlers(IServiceCollection services)
    {
        services.RegisterCommandHandler(provider =>
        {
            IEventServiceFacade eventServiceFacade = provider.GetRequiredService<IEventServiceFacade>();

            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new CreateApplicationHandler(
                eventServiceFacade,
                validationService,
                applicationExistenceService,
                applicationRepository
            ); ;
        });

        services.RegisterCommandHandler(provider =>
        {
            IEventServiceFacade eventServiceFacade = provider.GetRequiredService<IEventServiceFacade>();

            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new UpdateApplicationHandler(
                eventServiceFacade,
                applicationExistenceService,
                validationService,
                applicationRepository
            );
        });

        services.RegisterCommandHandler(provider =>
        {
            IEventServiceFacade eventServiceFacade = provider.GetRequiredService<IEventServiceFacade>();

            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new DeleteApplicationHandler(eventServiceFacade, applicationRepository);
        });
    }
}