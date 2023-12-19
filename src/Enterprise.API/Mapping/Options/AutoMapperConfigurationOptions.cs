using System.Reflection;

namespace Enterprise.API.Mapping.Options;

public class AutoMapperConfigurationOptions
{
    public bool EnableAutoMapper { get; set; }
    public Func<Assembly[]>? GetMappingProfileAssemblies { get; set; }

    public AutoMapperConfigurationOptions()
    {
        EnableAutoMapper = true;
        GetMappingProfileAssemblies = null;
    }
}