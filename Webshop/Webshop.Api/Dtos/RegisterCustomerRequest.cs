using Webshop.Api.Entities;

namespace Webshop.Api.Dtos;

public class RegisterCustomerRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? FullName { get; set; }

    public User ToEntity()
        => User.CreateUser(Email, Password, UserRole.Customer, FullName);
}
