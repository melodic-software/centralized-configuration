using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Enterprise.API.Middleware;

/// <summary>
/// This redirects to swagger middleware for requests to the root resource under certain conditions.
/// This middleware should only be registered if swagger documentation middleware has been registered.
/// </summary>
public class RootRedirectMiddleware
{
    private const string EmptyString = "";
    private const string RootPath = "/";
    private const string IndexHtml = "/index.html";

    private readonly RequestDelegate _next;
    private readonly ILogger<RootRedirectMiddleware> _logger;
    private readonly string? _swaggerRoutePrefix;

    public RootRedirectMiddleware(RequestDelegate next, ILogger<RootRedirectMiddleware> logger, string? swaggerRoutePrefix)
    {
        _next = next;
        _logger = logger;
        _swaggerRoutePrefix = swaggerRoutePrefix;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (string.IsNullOrEmpty(_swaggerRoutePrefix))
        {
            // the root API resource (if one exists) and generated swagger documentation are at the same path
            // in this case we can't signal to the swagger middleware that it should be bypassed
            await _next(context);
        }
        else
        {
            HttpRequest request = context.Request;
            IHeaderDictionary requestHeaders = request.Headers;
            StringValues authorizationHeader = requestHeaders.Authorization;
            PathString path = request.Path;
            RouteValueDictionary routeValues = request.RouteValues;

            bool noRouteValues = !routeValues.Any();
            bool isRoot = path is { HasValue: true, Value: RootPath or EmptyString };
            bool isIndexHtml = path.HasValue && path == IndexHtml;
            bool noAuthHeader = !authorizationHeader.Any();

            bool redirectToSwagger = noRouteValues && (isRoot || isIndexHtml) && noAuthHeader;

            // TODO: do we have a root API resource at "/"?
            // if so, we may not want to do the redirect, and need to check additional information
            // can we inspect the API explorer metadata here?

            if (redirectToSwagger)
            {
                context.Response.Redirect(_swaggerRoutePrefix, false);
            }
            else
            {
                await _next(context);
            }
        }
    }
}