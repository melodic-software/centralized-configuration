using System.Reflection;
using Enterprise.API.Mapping.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API.Mapping;

public static class AutoMapperConfigurationService
{
    public static void ConfigureAutoMapper(this IServiceCollection services, AutoMapperConfigurationOptions options)
    {
        if (!options.EnableAutoMapper)
            return;

        if (options.GetMappingProfileAssemblies == null)
            throw new ArgumentNullException(nameof(options.GetMappingProfileAssemblies));

        Assembly[] allAssemblies = options.GetMappingProfileAssemblies.Invoke();

        services.AddAutoMapper(allAssemblies);
    }
}