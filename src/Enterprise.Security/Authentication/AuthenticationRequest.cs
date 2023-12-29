namespace Enterprise.Security.Authentication;

public class AuthenticationRequest(string username, string password)
{
    public string Username { get; } = username;
    public string Password { get; } = password;
}