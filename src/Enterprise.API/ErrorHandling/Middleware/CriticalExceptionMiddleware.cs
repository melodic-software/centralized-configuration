using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace Enterprise.API.ErrorHandling.Middleware;

[Obsolete("Use IExceptionHandler instead of middleware. This was introduced with .NET 8.")]
public class CriticalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CriticalExceptionMiddleware> _logger;

    public CriticalExceptionMiddleware(RequestDelegate next, ILogger<CriticalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SqliteException sqliteException)
        {
            if (sqliteException.SqliteErrorCode == 551) // TODO: Add and reference a constant value.
            {
                _logger.LogCritical(sqliteException, "A fatal database error occurred!");
            }

            throw; // Rethrow and allow a higher middleware handle it (like the global exception handler).
        }
    }
}