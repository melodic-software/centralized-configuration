namespace Enterprise.Security.Authentication;

public class AuthenticationRequest
{
    public string Username { get; }
    public string Password { get; }

    public AuthenticationRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
}