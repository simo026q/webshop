namespace Webshop.Api.Dtos;

public class ChangePasswordRequest
{
    public string Password { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
