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
        {
            string fallbackWarning = "The delegate for returning mapping profile assemblies has not been configured. " +
                                     "Falling back to the default.";

            Console.WriteLine(fallbackWarning);
            options.GetMappingProfileAssemblies = GetDefaultAssemblies;
        }
            
        Assembly[] allAssemblies = options.GetMappingProfileAssemblies.Invoke();

        services.AddAutoMapper(allAssemblies);
    }

    private static Assembly[] GetDefaultAssemblies()
    {
        // We're going to assume that the mapping profiles are in the primary API project.
        Assembly? entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly == null)
            throw new Exception("Entry assembly is null. Cannot configure AutoMapper!");

        return [entryAssembly];
    }
}