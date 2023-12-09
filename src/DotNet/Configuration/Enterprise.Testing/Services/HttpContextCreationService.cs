using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Enterprise.Testing.Services;

public static class HttpContextCreationService
{
    /// <summary>
    /// Use this when you need an HttpContext instance but you don't need to change any of it's read-only properties.
    /// Some of the read-only properties include the features collection, connection, http request and response objects, etc.
    /// In the case that you do need to change these, use Moq for mocking an HttpContext instance.
    /// </summary>
    /// <returns></returns>
    public static HttpContext CreateDefaultContext() => new DefaultHttpContext();

    public static HttpContext CreateDefaultContext(ClaimsPrincipal claimsPrincipal) => new DefaultHttpContext
    {
        User = claimsPrincipal
    };
}