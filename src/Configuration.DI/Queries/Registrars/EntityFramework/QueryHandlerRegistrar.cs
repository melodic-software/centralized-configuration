using Configuration.ApplicationServices.Applications.GetApplicationById;
using Configuration.ApplicationServices.Applications.GetApplicationByUniqueName;
using Configuration.ApplicationServices.Applications.GetApplications;
using Configuration.ApplicationServices.Applications.Shared;
using Configuration.EntityFramework.Entities;
using Configuration.EntityFramework.Queries.Sorting;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.DI;
using Enterprise.Core.Queries.Sorting;
using Enterprise.Mapping.Properties.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Queries.Registrars.EntityFramework;

internal static class QueryHandlerRegistrar
{
    internal static void RegisterQueryHandlers(IServiceCollection services)
    {
        services.RegisterQueryHandler(provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IPropertyMappingService propertyMappingService = provider.GetRequiredService<IPropertyMappingService>();
            IValidateSort sortValidator = new SortValidator<ApplicationResult, ApplicationEntity>(propertyMappingService);
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            
            return new GetApplicationsHandler(appServiceDependencies, sortValidator, applicationRepository);
        });

        services.RegisterQueryHandler(provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            
            return new GetApplicationByIdHandler(appServiceDependencies, applicationRepository);
        });

        services.RegisterQueryHandler(provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            
            return new GetApplicationByUniqueNameHandler(appServiceDependencies, applicationRepository);
        });
    }
}