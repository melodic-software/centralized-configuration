using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;
using static Enterprise.API.Versioning.Constants.VersioningConstants;

namespace Enterprise.API.Versioning;

public static class ApiVersioningConfigurationService
{
    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        IApiVersioningBuilder apiVersioningBuilder = services.AddApiVersioning(options =>
        {
            // when false, a 400 response is returned when a version is not specified in the request
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true; // adds "api-supported-versions" response header

            // defaults to "api-version", but can be specified in constructor (something like "v")
            // the default ApiVersionReader instance is this query string reader
            //options.ApiVersionReader = new QueryStringApiVersionReader();

            // uses a url segment (like Google and Facebook) -> "api/v1/resource"
            //options.ApiVersionReader = new UrlSegmentApiVersionReader();

            // defaults to "application/json;v=2.0" but can be specified in the constructor (something like "version" or "v")
            //options.ApiVersionReader = new MediaTypeApiVersionReader();

            options.ApiVersionReader = new HeaderApiVersionReader(CustomVersionRequestHeader);

            // https://referbruv.com/blog/how-to-use-api-versioning-in-asp-net-core-with-swagger-ui
        });

        apiVersioningBuilder.AddApiExplorer(options =>
        {
            // NOTE: some of these options are shared and will already be set by "services.AddApiVersioning"
            options.GroupNameFormat = VersionGroupNameFormat;

            // only enable this if using the URL version reader
            options.SubstituteApiVersionInUrl = false; 
        });
    }
}