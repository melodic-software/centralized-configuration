using System.Reflection;

namespace Configuration.AutoMapper;

public static class AutoMapperConfig
{
    public static Func<Assembly[]> GetMappingProfileAssemblies = () =>
    {
        // this assembly lives in the project dedicated to AutoMapper mapping profiles
        Assembly mappingProfileAssembly = typeof(AutoMapperConfig).Assembly;

        return new[] { mappingProfileAssembly };
    };
}