using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace Enterprise.API.ErrorHandling.Middleware;

public class CriticalExceptionMiddleware(RequestDelegate next, ILogger<CriticalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (SqliteException sqliteException)
        {
            if (sqliteException.SqliteErrorCode == 551) // TODO: Add and reference a constant value.
            {
                logger.LogCritical(sqliteException, "A fatal database error occurred!");
            }

            throw; // Rethrow and allow a higher middleware handle it (like the global exception handler).
        }
    }
}