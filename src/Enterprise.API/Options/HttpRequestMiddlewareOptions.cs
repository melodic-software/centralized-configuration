using Microsoft.AspNetCore.Builder;

namespace Enterprise.API.Options;

public class HttpRequestMiddlewareOptions
{
    public Action<WebApplication>? AddCustomMiddleware { get; set; }

    public HttpRequestMiddlewareOptions()
    {
        AddCustomMiddleware = null;
    }
}