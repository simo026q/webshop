using Webshop.Api.Entities;

namespace Webshop.Api.Dtos;

public class UserCreateRequest
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? FullName { get; set; }
    public UserRole Role { get; set; }

    public User ToEntity()
    {
        return new User
        {
            Id = Id,
            Email = Email,
            FullName = FullName,
            Role = Role
        };
    }
}
