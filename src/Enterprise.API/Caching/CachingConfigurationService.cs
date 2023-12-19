using Enterprise.API.Caching.Options;
using Enterprise.API.Caching.Services;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API.Caching;

public static class CachingConfigurationService
{
    public static void ConfigureCaching(this IServiceCollection services, IConfiguration configuration, CachingConfigurationOptions cacheConfigOptions)
    {
        // uncomment for in memory (local/non-distributed) caching
        //services.AddMemoryCache();

        // configuration for "IDistributedCache" references

        if (cacheConfigOptions.EnableRedis)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString(cacheConfigOptions.RedisConnectionString);
                options.InstanceName = cacheConfigOptions.RedisInstanceName;
            });
        }

        // alternate configuration for "IDistributedCache" (not likely going to be used)
        // SQL traffic is not reduced - REDIS is better served for reducing SQL overhead
        //services.AddDistributedSqlServerCache(options =>
        //{
        //    string cacheConnectionString = string.Empty;
        //    string cacheSchema = "dbo";
        //    string cacheTableName = "Cache";

        //    options.ConnectionString = cacheConnectionString;
        //    options.SchemaName = cacheSchema;
        //    options.TableName = cacheTableName;
        //});
    }

    public static void ConfigureResponseCaching(this IServiceCollection services)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/performance/caching/response
        // NOTE: there are several requirements for response caching to work at an individual request level
        // https://learn.microsoft.com/en-us/aspnet/core/performance/caching/middleware
        //services.AddResponseCaching();

        // https://github.com/KevinDockx/HttpCacheHeaders
        // https://www.nuget.org/packages/Marvin.Cache.Headers
        // this is a custom package that adds support for ETags
        // it allows for the addition of HttpCache headers to responses (Cache-Control, Expires, ETag, Last-Modified)
        // and implements cache expiration and validation models

        // TODO: configure a REDIS cache provider
        // TODO: does this take into account requests with different custom version header values?

        // register services required by any custom provider delegates
        // these should only apply to "AddHttpCacheHeaders"
        services.AddTransient<CustomStoreKeyGenerator>();
        services.AddTransient<CustomETagGenerator>();

        services.AddHttpCacheHeaders(expirationModelOptions =>
            {
                // sets the "Cache-Control" response header (globally)
                // they can be set on the controller level by using the [HttpCacheExpiration] attribute

                expirationModelOptions.MaxAge = 60;
                expirationModelOptions.CacheLocation = CacheLocation.Private;
            }, validationModelOptions =>
            {
                // these are global options
                // they can be overridden at the controller/action level by using the [HttpCacheValidation] attribute

                // tells a cache that re-validation has to happen if a response becomes stale
                // this forces re-validation by the cache even if the client has decided that stale responses are OK for a specified amount of time
                // clients specify this by setting the Max-Stale directive

                validationModelOptions.MustRevalidate = true;
            }, 
            storeKeyGeneratorFunc: provider => provider.GetRequiredService<CustomStoreKeyGenerator>(),
            eTagGeneratorFunc: provider => provider.GetRequiredService<CustomETagGenerator>());

        // NOTE: ETags can be used as tokens/validators for optimistic concurrency strategy
        // either the client (or preferably intermediary cache server or CDN) sends an "If-Match" header
        // on mismatch, a 412 Precondition Failed will be returned
    }

    public static void ConfigureOutputCaching(this IServiceCollection services)
    {
        // uses e-tags and memory cache by default (non distributed)
        // for distributed caching, a custom implementation of "IOutputCacheStore" must be provided
        // https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-8.0
    }

    public static void UseCaching(this WebApplication app)
    {
        //UseResponseCompression(app);
        //UseResponseCaching(app);
        UseHttpCacheHeaders(app);
    }

    private static void UseResponseCompression(WebApplication app)
    {
        // the compression modules in IIS are better than the .NET core implementations
        // https://learn.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-8.0
        // uncomment if hosting directly on an HTTP.sys or Kestrel server
        //app.UseResponseCompression();
    }

    private static void UseResponseCaching(WebApplication app)
    {
        // response caching needs to be placed before controllers are added (which adds endpoint mapping)
        // we want to ensure that the cache middleware can serve something up before the MVC logic is routed to / executed
        //app.UseResponseCaching();
    }

    private static void UseHttpCacheHeaders(WebApplication app)
    {
        // adds support for ETags, must be added after response caching
        // https://github.com/KevinDockx/HttpCacheHeaders
        // https://www.nuget.org/packages/Marvin.Cache.Headers
        // TODO: does this take into account requests with different custom version header values?
        app.UseHttpCacheHeaders();
    }
}