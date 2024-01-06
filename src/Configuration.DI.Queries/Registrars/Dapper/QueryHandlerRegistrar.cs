using Configuration.ApplicationServices.Queries.Applications.GetApplicationById;
using Configuration.ApplicationServices.Queries.Applications.GetApplications;
using Configuration.ApplicationServices.Queries.Applications.GetByUniqueName;
using Configuration.Core.Queries.Model;
using Configuration.Core.Queries.Repositories;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;
using Enterprise.Core.Queries.Sorting;
using Enterprise.Mapping.Properties.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Queries.Registrars.Dapper;

internal static class QueryHandlerRegistrar
{
    internal static void RegisterQueryHandlers(IServiceCollection services)
    {
        services.AddTransient(provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IPropertyMappingService propertyMappingService = provider.GetRequiredService<IPropertyMappingService>();
            IValidateSort sortValidator = null; // TODO: create object instance

            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            IHandleQuery<GetApplications, GetApplicationsResult> getApplicationsHandler = new GetApplicationsHandler(appServiceDependencies, sortValidator, applicationRepository);
            return getApplicationsHandler;
        });

        services.AddTransient(provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            IHandleQuery<GetApplicationById, Application?> getApplicationByIdHandler = new GetApplicationByIdHandler(appServiceDependencies, applicationRepository);
            return getApplicationByIdHandler;
        });

        services.AddTransient(provider =>
        {
            IApplicationServiceDependencies appServiceDependencies = provider.GetRequiredService<IApplicationServiceDependencies>();
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            IHandleQuery<GetApplicationByUniqueName, Application?> getApplicationByUniqueNameHandler = new GetApplicationByUniqueNameHandler(appServiceDependencies, applicationRepository);
            return getApplicationByUniqueNameHandler;
        });
    }
}