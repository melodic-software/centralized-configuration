using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Enterprise.API.Middleware;

/// <summary>
/// If a user is authenticated, a logging scope is created to capture user information (username, subject, etc.).
/// </summary>
public class UserScopeMiddleware(RequestDelegate next, ILogger<UserScopeMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        bool userIsAuthenticated = context.User.Identity is { IsAuthenticated: true };

        if (userIsAuthenticated)
        {
            ClaimsPrincipal user = context.User;

            string? identityName = user.Identity?.Name ?? "N/A";
            string? subjectId = user.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject)?.Value;

            using (logger.BeginScope("User:{user}, SubjectId:{subject}", identityName, subjectId))
            {
                await next(context);
            }
        }
        else
        {
            await next(context);
        }
    }
}