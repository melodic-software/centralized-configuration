using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Enterprise.API.Cors;

public static class CorsPolicyService
{
    public static bool PolicyExists(CorsOptions options, string policyName)
    {
        CorsPolicy? existingPolicy = options.GetPolicy(policyName);
        bool policyExists = existingPolicy != null;
        return policyExists;
    }
}