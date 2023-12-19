using Enterprise.API.Constants;
using Enterprise.API.Security.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Enterprise.API.Security.ApiKey;

public static class ApiKeyService
{
    public static bool RequestContainsValidApiKey(HttpContext httpContext, IConfiguration configuration)
    {
        IHeaderDictionary requestHeaders = httpContext.Request.Headers;

        bool apiKeyPresentInHeader = requestHeaders.TryGetValue(SecurityConstants.CustomApiKeyHeader, out StringValues extractedApiKey);
        string? configuredApiKey = configuration[ConfigConstants.ApiKeyConfigKeyName];
        bool keysMatch = apiKeyPresentInHeader && extractedApiKey == configuredApiKey;

        return keysMatch;
    }
}