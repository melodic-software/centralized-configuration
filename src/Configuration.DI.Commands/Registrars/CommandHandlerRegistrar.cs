using Configuration.ApplicationServices.Applications.CreateApplication;
using Configuration.ApplicationServices.Commands.Applications;
using Configuration.ApplicationServices.Commands.Applications.Handlers;
using Configuration.Domain.Applications;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.DateTimes.Current.Abstract;
using Enterprise.MediatR.Adapters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Commands.Registrars;

internal static class CommandHandlerRegistrar
{
    internal static void RegisterCommandHandlers(IServiceCollection services)
    {
        services.AddTransient(provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            ICurrentDateTimeService currentDateTimeService = provider.GetRequiredService<ICurrentDateTimeService>();

            //IHandleCommand<CreateApplication> createApplicationHandler = new CreateApplicationHandler(appServiceDependencies, validationService, applicationExistenceService, applicationRepository, currentDateTimeService);

            IMediator mediator = provider.GetRequiredService<IMediator>();
            IHandleCommand<CreateApplication> createApplicationHandler = new CommandHandlerAdapter<CreateApplication>(appServiceDependencies, mediator);

            return createApplicationHandler;
        });

        services.AddTransient(provider =>
        {
            IApplicationServiceDependencies applicationServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            IHandleCommand<UpdateApplication> updateApplicationHandler = new UpdateApplicationHandler(applicationServiceDependencies, applicationExistenceService, validationService, applicationRepository);

            return updateApplicationHandler;
        });

        services.AddTransient(provider =>
        {
            IApplicationServiceDependencies applicationServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            IHandleCommand<DeleteApplication> deleteApplicationHandler = new DeleteApplicationHandler(applicationServiceDependencies, applicationRepository);

            return deleteApplicationHandler;
        });
    }
}