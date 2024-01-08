using Configuration.ApplicationServices.Applications.CreateApplication;
using Configuration.ApplicationServices.Applications.DeleteApplication;
using Configuration.ApplicationServices.Applications.UpdateApplication;
using Configuration.Domain.Applications;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Commands.Registrars;

internal static class CommandHandlerRegistrar
{
    internal static void RegisterCommandHandlers(IServiceCollection services)
    {
        services.RegisterCommandHandler(provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new CreateApplicationHandler(
                appServiceDependencies,
                validationService,
                applicationExistenceService,
                applicationRepository
            ); ;
        });

        services.RegisterCommandHandler(provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IApplicationExistenceService applicationExistenceService = provider.GetRequiredService<IApplicationExistenceService>();
            ApplicationValidationService validationService = new ApplicationValidationService();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new UpdateApplicationHandler(
                appServiceDependencies,
                applicationExistenceService,
                validationService,
                applicationRepository
            );
        });

        services.RegisterCommandHandler(provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();

            return new DeleteApplicationHandler(
                appServiceDependencies,
                applicationRepository
            );
        });
    }
}