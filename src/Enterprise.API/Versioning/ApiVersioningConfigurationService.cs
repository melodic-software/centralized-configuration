using Asp.Versioning;
using Enterprise.API.Versioning.Options;
using Microsoft.Extensions.DependencyInjection;
using static Enterprise.API.Versioning.Constants.VersioningConstants;

namespace Enterprise.API.Versioning;

public static class ApiVersioningConfigurationService
{
    public static void ConfigureApiVersioning(this IServiceCollection services, VersioningConfigurationOptions versioningConfigOptions)
    {
        IApiVersioningBuilder apiVersioningBuilder = services.AddApiVersioning(options =>
        {
            // When false, a 400 response is returned when a version is not specified in the request.
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true; // Adds the "api-supported-versions" response header.

            options.ApiVersionReader = BuildVersionReader(versioningConfigOptions);
        });

        apiVersioningBuilder.AddApiExplorer(options =>
        {
            // NOTE: Some of these options are shared and will already be set by "services.AddApiVersioning".
            options.GroupNameFormat = VersionGroupNameFormat;

            // Only enable this if using the URL version reader.
            options.SubstituteApiVersionInUrl = versioningConfigOptions.EnableUrlVersioning;
        });
    }

    private static IApiVersionReader BuildVersionReader(VersioningConfigurationOptions configOptions)
    {
        List<IApiVersionReader> apiVersionReaders = new List<IApiVersionReader>();

        if (configOptions.EnableUrlVersioning)
            apiVersionReaders.Add(new UrlSegmentApiVersionReader());

        if (configOptions.EnableQueryStringVersioning)
            apiVersionReaders.Add(new QueryStringApiVersionReader(VersionQueryStringParameterName));

        if (configOptions.EnableHeaderVersioning)
            apiVersionReaders.Add(new HeaderApiVersionReader(CustomVersionRequestHeader));

        if (configOptions.EnableMediaTypeVersioning)
            apiVersionReaders.Add(new MediaTypeApiVersionReader(MediaTypeVersionParameterName));

        return ApiVersionReader.Combine(apiVersionReaders.ToArray());
    }
}