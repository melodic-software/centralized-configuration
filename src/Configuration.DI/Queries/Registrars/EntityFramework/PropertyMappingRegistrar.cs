using Configuration.ApplicationServices.Applications.Shared;
using Configuration.DI.Queries.Mapping.EntityFramework;
using Configuration.EntityFramework.Entities;
using Enterprise.Mapping.Properties.Model;
using Enterprise.Mapping.Properties.Model.Abstract;
using Enterprise.Mapping.Properties.Services;
using Enterprise.Mapping.Properties.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Queries.Registrars.EntityFramework;

internal class PropertyMappingRegistrar
{
    internal static void RegisterPropertyMappings(IServiceCollection services)
    {
        // the transient lifetime scope is recommended for lightweight stateless services
        services.AddTransient(provider =>
        {
            IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>
            {
                new PropertyMapping<ApplicationResult, ApplicationEntity>(PropertyMappings.ApplicationMappings)
                // add any additional mappings here (add to static PropertyMappings class)
            };

            IPropertyMappingService propertyMappingService = new PropertyMappingService(propertyMappings);

            return propertyMappingService;
        });
    }
}