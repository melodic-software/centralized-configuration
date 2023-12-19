using Configuration.Core.Queries.Model;
using Configuration.DI.Queries.Mapping;
using Configuration.EntityFramework.Entities;
using Enterprise.Mapping.Properties.Model;
using Enterprise.Mapping.Properties.Model.Abstract;
using Enterprise.Mapping.Properties.Services;
using Enterprise.Mapping.Properties.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Queries.Registrars;

internal class PropertyMappingRegistrar
{
    internal static void RegisterPropertyMappings(IServiceCollection services)
    {
        // the transient lifetime scope is recommended for lightweight stateless services
        services.AddTransient(provider =>
        {
            IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>
            {
                new PropertyMapping<Application, ApplicationEntity>(PropertyMappings.ApplicationMappings)
                // add any additional mappings here (add to static PropertyMappings class)
            };

            IPropertyMappingService propertyMappingService = new PropertyMappingService(propertyMappings);

            return propertyMappingService;
        });
    }
}