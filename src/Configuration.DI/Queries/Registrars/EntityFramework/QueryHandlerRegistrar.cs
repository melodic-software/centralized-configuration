using Configuration.ApplicationServices.Applications.GetApplicationById;
using Configuration.ApplicationServices.Applications.GetApplicationByUniqueName;
using Configuration.ApplicationServices.Applications.GetApplications;
using Configuration.ApplicationServices.Applications.Shared;
using Configuration.EntityFramework.DbContexts.Configuration;
using Configuration.EntityFramework.Entities;
using Configuration.EntityFramework.Queries.Execution;
using Configuration.EntityFramework.Queries.Sorting;
using Enterprise.ApplicationServices.DI;
using Enterprise.ApplicationServices.Events;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.Core.Queries.Sorting;
using Enterprise.Mapping.Properties.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Configuration.DI.Queries.Registrars.EntityFramework;

internal static class QueryHandlerRegistrar
{
    internal static void RegisterQueryHandlers(IServiceCollection services)
    {
        services.RegisterQueryHandler(provider =>
        {
            IEventServiceFacade eventServiceFacade = provider.GetRequiredService<IEventServiceFacade>();
            ILogger<GetApplicationsHandler> logger = provider.GetRequiredService<ILogger<GetApplicationsHandler>>();

            IPropertyMappingService propertyMappingService = provider.GetRequiredService<IPropertyMappingService>();
            IValidateSort sortValidator = new SortValidator<ApplicationResult, ApplicationEntity>(propertyMappingService);
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            
            return new GetApplicationsHandler(eventServiceFacade, logger,sortValidator, applicationRepository);
        });

        services.RegisterSimpleQueryHandler(provider =>
        {
            ConfigurationContext dbContext = provider.GetRequiredService<ConfigurationContext>();
            IPropertyMappingService propertyMappingService = provider.GetRequiredService<IPropertyMappingService>();
            ILogger<GetApplicationByIdLogic> logger = provider.GetRequiredService<ILogger<GetApplicationByIdLogic>>();

            IQueryLogic<GetApplicationById, ApplicationResult?> queryLogic = new GetApplicationByIdLogic(dbContext, propertyMappingService, logger);

            return queryLogic;
        });

        services.RegisterQueryHandler(provider =>
        {
            IEventServiceFacade eventServiceFacade = provider.GetRequiredService<IEventServiceFacade>();
            ILogger<GetApplicationByUniqueNameHandler> logger = provider.GetRequiredService<ILogger<GetApplicationByUniqueNameHandler>>();

            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            
            return new GetApplicationByUniqueNameHandler(eventServiceFacade, logger, applicationRepository);
        });
    }
}