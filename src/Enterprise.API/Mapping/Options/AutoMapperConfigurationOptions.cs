using System.Reflection;

namespace Enterprise.API.Mapping.Options;

public class AutoMapperConfigurationOptions
{
    public bool EnableAutoMapper { get; set; } = false;
    public Func<Assembly[]>? GetMappingProfileAssemblies { get; set; } = null;
}