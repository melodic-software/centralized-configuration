using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Enterprise.Security.Tokens;

public class JwtTokenGenerationService
{
    public string GenerateTokenString(string issuer, string audience, IEnumerable<Claim> claims,
        DateTime? notBefore, DateTime? expiration, SigningCredentials signingCredentials)
    {
        JwtSecurityToken jwtSecurityToken = GenerateToken(issuer, audience, claims, notBefore, expiration, signingCredentials);
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        string? jwt = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
        return jwt;
    }

    public JwtSecurityToken GenerateToken(string issuer, string audience, IEnumerable<Claim> claims,
        DateTime? notBefore, DateTime? expiration, SigningCredentials signingCredentials)
    {
        if (expiration <= notBefore)
            throw new Exception("Expiration date must be after the \"not before\" date");

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            notBefore, // indicates the start of token validity (before this time the token cannot be used and validation will fail)
            expiration, // indicates the end of token validity (after this time the token cannot be used and validation will fail)
            signingCredentials
        );

        return jwtSecurityToken;
    }
}