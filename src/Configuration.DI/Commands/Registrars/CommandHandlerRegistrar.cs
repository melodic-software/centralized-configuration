using Configuration.ApplicationServices.Applications.CreateApplication;
using Configuration.ApplicationServices.Applications.DeleteApplication;
using Configuration.ApplicationServices.Applications.UpdateApplication;
using Configuration.Domain.Applications;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.DI;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Enterprise.Events.Services.Raising;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Configuration.DI.Commands.Registrars;

internal static class CommandHandlerRegistrar
{
    internal static void RegisterCommandHandlers(IServiceCollection services)
    {
        services.RegisterCommandHandler(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();
            ILogger<CommandHandler<CreateApplication>> logger = provider.GetRequiredService<ILogger<CommandHandler<CreateApplication>>>();

            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new CreateApplicationHandler(
                eventRaiser,
                eventCallbackService,
                logger,
                validationService,
                applicationExistenceService,
                applicationRepository
            ); ;
        });

        services.RegisterCommandHandler(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();
            ILogger<CommandHandler<UpdateApplication>> logger = provider.GetRequiredService<ILogger<CommandHandler<UpdateApplication>>>();

            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new UpdateApplicationHandler(
                eventRaiser,
                eventCallbackService,
                logger,
                applicationExistenceService,
                validationService,
                applicationRepository
            );
        });

        services.RegisterCommandHandler(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();
            ILogger<CommandHandler<DeleteApplication>> logger = provider.GetRequiredService<ILogger<CommandHandler<DeleteApplication>>>();

            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new DeleteApplicationHandler(
                eventRaiser,
                eventCallbackService,
                logger,
                applicationRepository
            );
        });
    }
}