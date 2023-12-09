using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Enterprise.Logging.Middleware;

/// <summary>
/// If a user is authenticated, a logging scope is created to capture user information (username, subject, etc.).
/// </summary>
public class UserScopeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserScopeMiddleware> _logger;

    public UserScopeMiddleware(RequestDelegate next, ILogger<UserScopeMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        bool userIsAuthenticated = context.User.Identity is { IsAuthenticated: true };

        if (userIsAuthenticated)
        {
            ClaimsPrincipal user = context.User;

            string? identityName = user.Identity?.Name ?? "N/A";
            string? subjectId = user.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject)?.Value;

            using (_logger.BeginScope("User:{user}, SubjectId:{subject}", identityName, subjectId))
            {
                await _next(context);
            }
        }
        else
        {
            await _next(context);
        }
    }
}