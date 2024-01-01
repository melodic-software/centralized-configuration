﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Enterprise.API.ErrorHandling.ExceptionHandlers;

public class TimeOutExceptionHandler(ILogger<TimeOutExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
            logger.LogError(exception, "A timeout occurred.");

            if (exception is not TimeoutException)
                return false;

            int statusCode = StatusCodes.Status408RequestTimeout;

            httpContext.Response.StatusCode = statusCode;

            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = statusCode, 
                Type = exception.GetType().Name,
                Title = "A timeout occurred.",
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

            return true;
        }
}