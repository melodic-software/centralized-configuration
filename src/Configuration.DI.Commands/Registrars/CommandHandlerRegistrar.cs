using Configuration.ApplicationServices.Commands.Applications;
using Configuration.ApplicationServices.Commands.Applications.Handlers;
using Configuration.Core.Domain.Services.Abstract;
using Configuration.Core.Domain.Services.Abstract.Repositories;
using Configuration.Core.Domain.Services.Validation;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Commands.Registrars;

internal static class CommandHandlerRegistrar
{
    internal static void RegisterCommandHandlers(IServiceCollection services)
    {
        services.AddTransient(provider =>
        {
            IApplicationServiceDependencies applicationServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            IHandleCommand<CreateApplication> createApplicationHandler = new CreateApplicationHandler(applicationServiceDependencies, validationService, applicationExistenceService, applicationRepository);

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