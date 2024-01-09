using Configuration.ApplicationServices.Applications.GetApplicationById;
using Configuration.ApplicationServices.Applications.GetApplicationByUniqueName;
using Configuration.ApplicationServices.Applications.GetApplications;
using Configuration.ApplicationServices.Applications.Shared;
using Configuration.Dapper.Queries.Execution;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.Core.Queries.Sorting;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Enterprise.Mapping.Properties.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Configuration.DI.Queries.Registrars.Dapper;

internal static class QueryHandlerRegistrar
{
    internal static void RegisterQueryHandlers(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>(); 
            
            IPropertyMappingService propertyMappingService = provider.GetRequiredService<IPropertyMappingService>();
            IValidateSort sortValidator = null; // TODO: create object instance

            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            IHandleQuery<GetApplications, GetApplicationsResult> getApplicationsHandler = new GetApplicationsHandler(eventRaiser, eventCallbackService, sortValidator, applicationRepository);
            return getApplicationsHandler;
        });

        services.AddTransient(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();

            // TODO: use shared constant (currently in API project)
            string? connectionString = configuration.GetConnectionString("SQLConnection");
            ILogger<GetApplicationByIdLogic> logger = provider.GetRequiredService<ILogger<GetApplicationByIdLogic>>();

            IQueryLogic<GetApplicationById, ApplicationResult?> queryLogic = new GetApplicationByIdLogic(connectionString, logger);

            IHandleQuery<GetApplicationById, ApplicationResult?> getApplicationByIdHandler = new SimpleQueryHandler<GetApplicationById, ApplicationResult?>(eventRaiser, eventCallbackService, queryLogic);

            return getApplicationByIdHandler;
        });

        services.AddTransient(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();

            IApplicationRepository applicationRepository = provider.GetRequiredService<IApplicationRepository>();
            IHandleQuery<GetApplicationByUniqueName, ApplicationResult?> getApplicationByUniqueNameHandler = new GetApplicationByUniqueNameHandler(eventRaiser, eventCallbackService, applicationRepository);
            return getApplicationByUniqueNameHandler;
        });
    }
}