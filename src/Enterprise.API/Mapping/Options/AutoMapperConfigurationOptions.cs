using System.Reflection;

namespace Enterprise.API.Mapping.Options;

public class AutoMapperConfigurationOptions
{
    public bool EnableAutoMapper { get; set; } = true;
    public Func<Assembly[]>? GetMappingProfileAssemblies { get; set; } = null;
}