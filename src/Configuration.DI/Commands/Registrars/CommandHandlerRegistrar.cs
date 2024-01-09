using Configuration.ApplicationServices.Applications.CreateApplication;
using Configuration.ApplicationServices.Applications.DeleteApplication;
using Configuration.ApplicationServices.Applications.UpdateApplication;
using Configuration.Domain.Applications;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
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
            // TODO: I'd like to somehow get these lines out into either the shared ^ register command handler method
            // OR just so I don't have to do this for every handler instantiation / type.
            IEventServiceFacade eventServiceFacade = provider.GetRequiredService<IEventServiceFacade>();
            ILogger<CommandHandler<CreateApplication>> logger = provider.GetRequiredService<ILogger<CommandHandler<CreateApplication>>>();

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
            ILogger<CommandHandler<UpdateApplication>> logger = provider.GetRequiredService<ILogger<CommandHandler<UpdateApplication>>>();

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
            ILogger<CommandHandler<DeleteApplication>> logger = provider.GetRequiredService<ILogger<CommandHandler<DeleteApplication>>>();

            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new DeleteApplicationHandler(
                eventServiceFacade,
                logger,
                applicationRepository
            );
        });
    }
}