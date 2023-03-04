using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Webshop.Api.Auth;
using Webshop.Api.Entities;
using Webshop.Api.Repositories;

namespace Webshop.Api.Services;

public class TokenManagerService : ITokenManagerService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly byte[] _key;

    public TokenManagerService(IUserRepository userRepository, IConfiguration configuration) 
        : this(userRepository, configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not set in configuration"))
    {
    }

    // Added for testing purposes
    public TokenManagerService(IUserRepository userRepository, string key)
    {
        _userRepository = userRepository;

        _key = Encoding.ASCII.GetBytes(key);
    }

    private SecurityTokenDescriptor GetTokenDescriptor(User user)
    {
        return new()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha512Signature)
        };
    }
    
    public async Task<string?> GenerateTokenAsync(AuthRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null || !(user.IsActive ?? false))
            return null;

        if (!user.CheckPassword(request.Password))
            return null;

        var descriptor = GetTokenDescriptor(user);

        var securityToken = _tokenHandler.CreateJwtSecurityToken(descriptor);

        return _tokenHandler.WriteToken(securityToken);
    }
}


public interface ITokenManagerService
{
    public Task<string?> GenerateTokenAsync(AuthRequest request);
}