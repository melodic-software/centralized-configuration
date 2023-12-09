using Microsoft.AspNetCore.Authorization;
using static Enterprise.API.Security.Constants.SecurityConstants;

namespace Enterprise.API.Security.Authorization.Options;

public static class AuthorizationOptionsExtensions
{
    /// <summary>
    /// This is the policy applied when an [Authorize] attribute has been applied
    /// or an [AllowAnonymous] attribute is present and no policy has been specified.
    /// </summary>
    /// <param name="options"></param>
    public static void AddDefaultPolicy(this AuthorizationOptions options)
    {
        // you can put these in the constructor of the AuthorizationPolicyBuilder
        string[] authenticationSchemes = { "Bearer", BasicAuthenticationScheme };

        AuthorizationPolicy defaultPolicy = new AuthorizationPolicyBuilder() // <- multiple checks can be applied
            //.RequireClaim(JwtClaimTypes.Role, "contributor") // <- example claim check
            .RequireAuthenticatedUser()
            // ^ the [Authorize] attribute has this requirement built in, but for a global policy this must be explicitly added
            .Build();

        // this is the policy applied when no [Authorize] attribute is applied AND no [AllowAnonymous] attribute present
        options.DefaultPolicy = defaultPolicy;
    }

    /// <summary>
    /// This is the policy applied when no [Authorize] attribute is applied AND no [AllowAnonymous] attribute is present.
    /// </summary>
    /// <param name="options"></param>
    public static void AddDefaultFallbackPolicy(this AuthorizationOptions options)
    {
        // you can put these in the constructor of the AuthorizationPolicyBuilder
        string[] authenticationSchemes = { "Bearer", BasicAuthenticationScheme };

        AuthorizationPolicy defaultFallbackPolicy = new AuthorizationPolicyBuilder() // <- multiple checks can be applied
            //.RequireClaim(JwtClaimTypes.Role, "contributor") // <- example claim check
            .RequireAuthenticatedUser()
            // ^ the [Authorize] attribute has this requirement built in, but for a global policy this must be explicitly added
            .Build();

        // by default this is null
        // TODO: make this configurable, keep the default as null
        options.FallbackPolicy = defaultFallbackPolicy;
    }
}