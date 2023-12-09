using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Enterprise.API.Security.Basic;

// https://app.pluralsight.com/course-player?clipId=e8b9e63a-24c1-49bd-b631-621e7f64e520

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail("Authorization header is missing"));

        StringValues authorizationHeader = Request.Headers["Authorization"];
        AuthenticationHeaderValue authorizationHeaderValue = AuthenticationHeaderValue.Parse(authorizationHeader);

        if (string.IsNullOrWhiteSpace(authorizationHeaderValue.Parameter))
            return Task.FromResult(AuthenticateResult.Fail("Authorization header parameter is invalid"));

        if (authorizationHeaderValue.Scheme == "Bearer")
        {
            // not the right scheme...
            return Task.FromResult(AuthenticateResult.Fail("Invalid scheme"));
        }
        else
        {
            // TODO: any other schemes to guard? can we validate it's a base 64 string value?
        }

        byte[] credentialByes = Convert.FromBase64String(authorizationHeaderValue.Parameter);
        string credentials = Encoding.UTF8.GetString(credentialByes);

        if (!credentials.Contains(":"))
            return Task.FromResult(AuthenticateResult.Fail("Authorization scheme is invalid"));

        string[] credentialsSplit = credentials.Split(':');

        if (credentialsSplit.Length != 2)
            return Task.FromResult(AuthenticateResult.Fail("Authorization scheme is invalid"));

        string username = credentialsSplit[0];
        string password = credentialsSplit[1];

        // TODO: replace with authentication service that can handle credential verification
        if (username == "admin" && password == "password")
        {
            Claim[] claims = new[] { new Claim(ClaimTypes.NameIdentifier, username) };
            ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));
    }
}