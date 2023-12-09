using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Security.Authorization.WindowsAuth.Extensions;

public static class WindowsAuthExtensions
{
    public static void AddWindowsAuth(this AuthenticationBuilder authBuilder)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/security/authentication/windowsauth

        authBuilder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate(options =>
        {

        });

        authBuilder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = options.DefaultPolicy;
        });
    }
}