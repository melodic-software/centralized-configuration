using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Enterprise.API.Client;

public static class RestApiClientOperations
{
    public static async Task HandleInvalidResponse(HttpResponseMessage response, HttpClient httpClient, string resourceUri, ILogger logger, HttpContext? httpContext = null, ClaimsPrincipal? claimsPrincipal = null)
    {
        if (httpContext != null)
        {
            string? accessToken = await httpContext.GetTokenAsync("access_token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // for a better way to include and manage access tokens for API calls:
            // https://identitymodel.readthedocs.io/en/latest/aspnetcore/web.html
        }

        if (!response.IsSuccessStatusCode)
        {
            int statusCode = (int)response.StatusCode;
            string fullPath = $"{httpClient.BaseAddress}{resourceUri}";

            // trace ID
            ProblemDetails details = await response.Content.ReadFromJsonAsync<ProblemDetails>() ?? new ProblemDetails();
            string traceId = details.Extensions["traceId"]?.ToString() ?? string.Empty;

            string? username = claimsPrincipal?.Identity?.IsAuthenticated ?? false ? claimsPrincipal.Identity.Name : string.Empty;

            logger.LogWarning("API Failure: {fullPath} API Response: {apiResponse}, Trace: {trace}, Username: {username}", 
                fullPath, statusCode, traceId, username);
        }
    }
}