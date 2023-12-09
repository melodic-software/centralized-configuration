using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using static Enterprise.API.Security.ApiKey.ApiKeyService;
using static Enterprise.API.Swagger.Services.SwaggerRequestDetectionService;

namespace Enterprise.API.Security.ApiKey.Attributes;

// use EITHER the middleware OR this attribute
// this attribute could be extended to allow for multiple keys (one for each external party)
// all keys could exist in configuration, or a database

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ApiKeyAttribute : Attribute, IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    // NOTE: since we're injecting into the constructor, this attribute can't be used directly
    // it must be referenced as a type param in the [TypeFilter] attribute (this allows for DI to work)
    public ApiKeyAttribute(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        HttpContext httpContext = context.HttpContext;

        if (RequestContainsValidApiKey(httpContext, _configuration))
            return;

        if (SwaggerPageRequested(httpContext))
            return;

        context.Result = new UnauthorizedResult();
    }
}