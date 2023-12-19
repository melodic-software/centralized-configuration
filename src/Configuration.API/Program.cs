using Configuration.API.Client;
using Configuration.API.Constants;
using Configuration.API.Dependencies;
using Configuration.API.EntityFramework;
using Configuration.API.Redis.Constants;
using Configuration.API.Security.Constants;
using Configuration.API.Swagger;
using Configuration.API.Swagger.Constants;
using Configuration.AutoMapper;
using Enterprise.API;
using Enterprise.API.Caching.Options;
using Enterprise.API.Events;
using Enterprise.API.Mapping.Options;
using Enterprise.API.Options;
using Enterprise.API.Security.Options;
using Enterprise.API.Swagger.Options;
using Enterprise.Logging.Options;
using Enterprise.Monitoring.Health.Options;

ApiConfigurationService.Configure(args, options =>
{
    options.EventHandlers = new DefaultApiConfigurationEventHandlers();

    options.EventHandlers.OnRequestPipelineConfigured += async app =>
    {
        await EntityFrameworkConfiguration.OnRequestPipelineConfigured(app);
    };

    options.HealthCheckOptions = new HealthCheckOptions
    {
        OpenIdConnectAuthorityUri = SecurityConstants.Authority,
        AddHealthChecks = builder =>
        {
            EntityFrameworkConfiguration.AddDbContextHealthChecks(builder);
            // add additional health checks here
        }
    };

    options.HttpRequestMiddlewareOptions = new HttpRequestMiddlewareOptions
    {

    };

    options.LoggingConfigurationOptions = new LoggingConfigurationOptions
    {
        LogFileApplicationName = ApplicationConstants.ApplicationName,
        //AddLogFilters = builder => builder.AddFilter("Configuration", LogLevel.Debug)
    };
    
    options.ServiceConfigurationOptions = new ServiceConfigurationOptions
    {
        AutoMapperConfigurationOptions = new AutoMapperConfigurationOptions
        {
            EnableAutoMapper = true,
            GetMappingProfileAssemblies = AutoMapperConfig.GetMappingProfileAssemblies
        },
        CachingConfigurationOptions = new CachingConfigurationOptions
        {
            EnableRedis = false,
            RedisConnectionString = RedisConstants.RedisConnectionStringName,
            RedisInstanceName = RedisConstants.RedisInstanceName
        },
        JwtBearerTokenOptions = new JwtBearerTokenOptions
        {
            // TODO: all this can be defaulted (either pre-built options objects OR ultimately non-configurable (hardcoded - this goes away)
            Authority = SecurityConstants.Authority,
            Audience = SecurityConstants.Audience,
            NameClaimType = SecurityConstants.NameClaimType,
            ConfigureJwtBearerOptions = null
        },
        RegisterCustomServices = ApplicationDependencyRegistrar.RegisterCustomServices
    };

    options.SwaggerConfigurationOptions = new SwaggerConfigurationOptions
    {
        EnableSwagger = true,
        Authority = SecurityConstants.Authority,
        OAuthClientId = SwaggerConstants.OAuthClientId,
        OAuthScopes = SecurityConstants.OAuthScopes,
        OAuthAppName = SwaggerConstants.OAuthAppName,
        ApplicationName = ApplicationConstants.ApplicationDisplayName,
        ApplicationDescription = ApplicationConstants.ApplicationDescription,
        GetApiClientAssembly = ApiClientConfig.GetAssembly,
        PostConfigure = SwaggerConfig.PostConfigure
    };
});

// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests
namespace Configuration.API
{
    public partial class Program { }
}