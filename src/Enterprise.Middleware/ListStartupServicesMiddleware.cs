using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Middleware;

public class ListStartupServicesMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceCollection _services;
    private const string Path = "/debug/all-registered-services";

    public ListStartupServicesMiddleware(RequestDelegate next, IServiceCollection services)
    {
        _next = next;
        _services = services;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        // TODO: do we just want to convert this to an API resource?
        // is that even possible?

        if (httpContext.Request.Path == Path)
        {
            // TODO: add query string parameters to filter by namespace
            // this can be prebuilt like "show all custom" (namespaces other than System.*, Microsoft.*, etc.)
            // or vice versa

            // TODO: make a model class for this?
            var result = _services.Select(x =>
            {
                string? instanceTypeName = null;

                if (x.ImplementationInstance != null)
                {
                    instanceTypeName = x.ImplementationInstance.GetType().FullName;
                }
                else if (x.ImplementationFactory != null)
                {
                    object instance = x.ImplementationFactory.Invoke(httpContext.RequestServices);
                    instanceTypeName = instance.GetType().FullName;
                }

                return new
                {
                    Type = x.ServiceType.FullName,
                    Lifetime = x.Lifetime, // TODO: translate to human readable display value
                    Instance = instanceTypeName
                };
            }).OrderBy(x => x.Type).ToList();

            // TODO: do we want to group by source type namespace?

            JsonSerializerDefaults serializationDefaults = JsonSerializerDefaults.Web;
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(serializationDefaults);
            string json = JsonSerializer.Serialize(result, jsonSerializerOptions);

            httpContext.Response.ContentType = MediaTypeNames.Application.Json;

            await httpContext.Response.WriteAsync(json);
        }
        else
        {
            await _next(httpContext);
        }
    }
}