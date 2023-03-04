using Webshop.Api.Entities;

namespace Webshop.Api.Dtos;

public class UserResponse
{
    public Guid Id { get; set; }
    public Guid? AddressId { get; set; }
    public string Email { get; set; }
    public string? FullName { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public static UserResponse? FromEntity(User? user)
    {
        if (user == null)
            return null;

        return new UserResponse
        {
            Id = user.Id,
            AddressId = user.AddressId,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            IsActive = user.IsActive ?? true,
            CreatedAt = user.CreatedAt
        };
    }
}
