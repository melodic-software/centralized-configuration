using System.Security.Claims;
using Enterprise.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Enterprise.Security.Tests.Tokens;

public class JwtTokenTests
{
    [Fact]
    public void JwtTokenTest()
    {
        SecurityKeyGenerationService securityKeyGenerationService = new SecurityKeyGenerationService();
        SigningCredentialGenerationService signingCredentialGenerationService = new SigningCredentialGenerationService();
        JwtTokenGenerationService jwtTokenGenerationService = new JwtTokenGenerationService();

        // has to be at least 128 bytes long
        string secret = "thisisthesecretforgeneratingakey(mustbeatleast32bitlong)";

        SymmetricSecurityKey securityKey = securityKeyGenerationService.GenerateSecurityKey(secret);

        // NOTE: the encryption algorithm 'HS256' requires a key size of at least '128' bits
        SigningCredentials signingCredentials = signingCredentialGenerationService.Generate(securityKey);

        int userId = 1;
        string userFirstName = "John";
        string userLastName = "Doe";
        string city = "New York City";

        List<Claim> claims = new List<Claim>
        {
            new("sub", userId.ToString()),
            new("given_name", userFirstName),
            new("family_name", userLastName),
            new("city", city)
        };

        string issuer = "https://security-token-service:12345";
        string audience = "configuration-api";
        DateTime notBefore = DateTime.UtcNow;
        DateTime expiration = notBefore.AddHours(1);

        string token = jwtTokenGenerationService.GenerateTokenString(issuer, audience, claims, notBefore, expiration, signingCredentials);

        Assert.NotNull(token);
    }
}