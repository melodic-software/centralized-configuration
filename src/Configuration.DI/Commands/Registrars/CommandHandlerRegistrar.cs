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
            ILogger<CommandHandlerBase<CreateApplication>> logger = provider.GetRequiredService<ILogger<CommandHandlerBase<CreateApplication>>>();

            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new CreateApplicationHandler(
                eventServiceFacade,
                logger,
                validationService,
                applicationExistenceService,
                applicationRepository
            ); ;
        });

        services.RegisterCommandHandler(provider =>
        {
            IEventServiceFacade eventServiceFacade = provider.GetRequiredService<IEventServiceFacade>();
            ILogger<CommandHandlerBase<UpdateApplication>> logger = provider.GetRequiredService<ILogger<CommandHandlerBase<UpdateApplication>>>();

            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new UpdateApplicationHandler(
                eventServiceFacade,
                logger,
                applicationExistenceService,
                validationService,
                applicationRepository
            );
        });

        services.RegisterCommandHandler(provider =>
        {
            IEventServiceFacade eventServiceFacade = provider.GetRequiredService<IEventServiceFacade>();
            ILogger<CommandHandlerBase<DeleteApplication>> logger = provider.GetRequiredService<ILogger<CommandHandlerBase<DeleteApplication>>>();

            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new DeleteApplicationHandler(
                eventServiceFacade,
                logger,
                applicationRepository
            );
        });
    }
}