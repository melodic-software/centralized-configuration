using Microsoft.IdentityModel.Tokens;

namespace Enterprise.Security.Tokens;

public class SigningCredentialGenerationService
{
    public SigningCredentials Generate(SymmetricSecurityKey securityKey)
    {
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        return signingCredentials;
    }
}