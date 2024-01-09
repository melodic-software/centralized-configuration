using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Enterprise.API.Middleware;
// https://stackoverflow.com/questions/67090158/how-to-ignore-favicon-call-in-asp-net-5-web-api

/// <summary>
/// Most browsers default to requesting a favicon whenever the user visits a new website.
/// This can trigger custom middleware an additional time, causing confusion.
/// 
/// </summary>
public class IgnoreFaviconMiddleware
{
    private readonly ILogger<RootRedirectMiddleware> _logger;
    private readonly RequestDelegate _next;

    /// <summary>
    /// Most browsers default to requesting a favicon whenever the user visits a new website.
    /// This can trigger custom middleware an additional time, causing confusion.
    /// 
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public IgnoreFaviconMiddleware(RequestDelegate next, ILogger<RootRedirectMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Value == "/favicon.ico")
        {
            // Here we're inspecting the path, and returning a 404.
            // This ensures subsequent middleware will not process the request.
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        await _next.Invoke(context);
    }
}