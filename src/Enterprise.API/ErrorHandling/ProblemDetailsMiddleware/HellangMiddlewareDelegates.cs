using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace Enterprise.API.ErrorHandling.ProblemDetailsMiddleware
{
    public static class HellangMiddlewareDelegates
    {
        /// <summary>
        /// This is using the default behavior, with some exceptions.
        /// Specific status codes will not result in a problem details response.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static bool IsProblem(HttpContext httpContext)
        {
            // Status code must be between 400 and 600.
            if (httpContext.Response.StatusCode is < 400 or > 600)
                return false;

            // Content-Length must be empty.
            if (!string.IsNullOrEmpty(httpContext.Response.Headers[HeaderNames.ContentLength]) ||
                httpContext.Response.ContentLength is null or <= 0)
                return false;

            // Content-Type must be empty.
            if (!string.IsNullOrEmpty(httpContext.Response.Headers[HeaderNames.ContentType]))
                return false;

            // We make an exception for 401 responses
            // This was done because the Swagger UI was experiencing 406 NotAcceptable responses when using a versioned media type.
            // This was observed with a multi-document setup. Authorization context is lost when switching (unless configured to be persisted/cached).
            if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
                return false;

            return true;
        }
    }
}
