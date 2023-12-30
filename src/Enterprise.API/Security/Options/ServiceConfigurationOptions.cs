using Enterprise.API.Caching.Options;
using Enterprise.API.Controllers.Options;
using Enterprise.API.Hosting.Options;
using Enterprise.API.Mapping.Options;
using Enterprise.API.Versioning.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API.Security.Options;

public class ServiceConfigurationOptions
{
    public AutoMapperConfigurationOptions AutoMapperConfigurationOptions { get; set; } = new();
    public CachingConfigurationOptions CachingConfigurationOptions { get; set; } = new();
    public IISIntegrationOptions IISIntegrationOptions { get; set; } = new();
    public JwtBearerTokenOptions JwtBearerTokenOptions { get; set; } = new();
    public ControllerConfigurationOptions ControllerConfigurationOptions { get; set; } = new();
    public Action<IServiceCollection, WebApplicationBuilder>? RegisterCustomServices { get; set; } = (_, _) => { };
    public VersioningConfigurationOptions VersioningConfigurationOptions { get; set; } = new();
}