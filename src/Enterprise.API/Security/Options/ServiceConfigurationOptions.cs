using Enterprise.API.Caching.Options;
using Enterprise.API.Controllers.Options;
using Enterprise.API.Hosting.Options;
using Enterprise.API.Mapping.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API.Security.Options;

public class ServiceConfigurationOptions
{
    public AutoMapperConfigurationOptions AutoMapperConfigurationOptions { get; set; }
    public CachingConfigurationOptions CachingConfigurationOptions { get; set; }
    public IISIntegrationOptions IISIntegrationOptions { get; set; }
    public JwtBearerTokenOptions JwtBearerTokenOptions { get; set; }
    public ControllerConfigurationOptions ControllerConfigurationOptions { get; set; }
    public Action<IServiceCollection, WebApplicationBuilder>? RegisterCustomServices { get; set; }

    public ServiceConfigurationOptions()
    {
        AutoMapperConfigurationOptions = new AutoMapperConfigurationOptions();
        CachingConfigurationOptions = new CachingConfigurationOptions();
        IISIntegrationOptions = new IISIntegrationOptions();
        JwtBearerTokenOptions = new JwtBearerTokenOptions();
        ControllerConfigurationOptions = new ControllerConfigurationOptions();
        RegisterCustomServices = (_, _) => { };
    }
}