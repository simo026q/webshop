using Webshop.Api.Entities;

namespace Webshop.Api.Dtos;

public class UserUpdateRequest
{
    public Guid Id { get; set; }
    public Guid? AddressId { get; set; }
    public string? FullName { get; set; }
    public bool IsActive { get; set; }

    public User ToEntity(User user)
    {
        if (Id != user.Id)
            return user;

        user.AddressId = AddressId;
        user.FullName = FullName;
        user.IsActive = IsActive;

        return user;
    }
}
