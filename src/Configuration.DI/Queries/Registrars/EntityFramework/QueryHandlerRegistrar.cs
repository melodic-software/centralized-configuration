using Configuration.ApplicationServices.Applications.GetApplicationById;
using Configuration.ApplicationServices.Applications.GetApplicationByUniqueName;
using Configuration.ApplicationServices.Applications.GetApplications;
using Configuration.ApplicationServices.Applications.Shared;
using Configuration.EntityFramework.DbContexts.Configuration;
using Configuration.EntityFramework.Entities;
using Configuration.EntityFramework.Queries.Execution;
using Configuration.EntityFramework.Queries.Sorting;
using Enterprise.ApplicationServices.DI;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.Core.Queries.Sorting;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
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
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();

            IPropertyMappingService propertyMappingService = provider.GetRequiredService<IPropertyMappingService>();
            IValidateSort sortValidator = new SortValidator<ApplicationResult, ApplicationEntity>(propertyMappingService);
            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            
            return new GetApplicationsHandler(eventRaiser, eventCallbackService, sortValidator, applicationRepository);
        });

        services.RegisterQueryHandler(provider =>
        {
            ConfigurationContext dbContext = provider.GetRequiredService<ConfigurationContext>();
            IPropertyMappingService propertyMappingService = provider.GetRequiredService<IPropertyMappingService>();
            ILogger<GetApplicationByIdLogic> logger = provider.GetRequiredService<ILogger<GetApplicationByIdLogic>>();

            IQueryLogic<GetApplicationById, ApplicationResult?> queryLogic = new GetApplicationByIdLogic(dbContext, propertyMappingService, logger);

            return queryLogic;
        });

        services.RegisterQueryHandler(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();

            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            
            return new GetApplicationByUniqueNameHandler(eventRaiser, eventCallbackService, applicationRepository);
        });
    }
}