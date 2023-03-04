using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using Webshop.Api.Interfaces;

namespace Webshop.Api.Entities;

public class User : IUniqueEntity<Guid>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid? AddressId { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public UserRole Role { get; set; }
    public bool? IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Address? Address { get; set; }

    public void UpdatePassword(string rawPassword)
    {
        PasswordSalt = Guid.NewGuid().ToString().Replace("-", "");
        
        PasswordHash = HashString(rawPassword, PasswordSalt);
    }

    public bool CheckPassword(string rawPassword)
    {
        return HashString(rawPassword, PasswordSalt) == PasswordHash;
    }

    private static string HashString(string value, string salt)
    {
        using var algorithm = SHA512.Create();

        var hashedValueBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value + salt));
        
        return Convert.ToBase64String(hashedValueBytes);
    }

    public static User CreateUser(string email, string password, UserRole role, string? fullName = null, Guid? id = null)
    {
        User user = new()
        {
            Id = id ?? Guid.NewGuid(),
            Email = email,
            FullName = fullName,
            Role = role,
            IsActive = true,
            PasswordHash = string.Empty,
            PasswordSalt = string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        user.UpdatePassword(password);

        return user;
    }
}

public enum UserRole
{
    Customer,
    Employee,
    Admin
}