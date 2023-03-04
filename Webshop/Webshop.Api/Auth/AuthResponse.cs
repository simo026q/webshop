namespace Webshop.Api.Auth;

public class AuthResponse
{
    public AuthResponse(string? token)
    {
        Token = token;
        Success = token != null;
    }
    
    public string? Token { get; set; }
    public bool Success { get; set; }
}
