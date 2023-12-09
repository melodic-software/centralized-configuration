using Enterprise.API.Swagger.Constants;
using Microsoft.AspNetCore.Http;

namespace Enterprise.API.Swagger.Services;

public static class SwaggerRequestDetectionService
{
    public static bool SwaggerPageRequested(HttpContext httpContext)
    {
        PathString requestPath = httpContext.Request.Path;

        string swaggerRoutePrefix = SwaggerConstants.RoutePrefix;
        bool isSwagger;

        if (string.IsNullOrWhiteSpace(SwaggerConstants.RoutePrefix))
        {
            isSwagger = requestPath == string.Empty;
        }
        else
        {
            if (!SwaggerConstants.RoutePrefix.StartsWith("/"))
                swaggerRoutePrefix = $"/{swaggerRoutePrefix}";

            isSwagger = httpContext.Request.Path.StartsWithSegments(swaggerRoutePrefix);
        }

        return isSwagger;
    }
}