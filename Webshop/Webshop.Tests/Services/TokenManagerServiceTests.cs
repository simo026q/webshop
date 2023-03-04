using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Api.Auth;
using Webshop.Api.Entities;
using Webshop.Api.Repositories;
using Webshop.Api.Services;
using Xunit;

namespace Webshop.Tests.Services;

public class TokenManagerServiceTests
{
    private readonly TokenManagerService _tokenManager;
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public TokenManagerServiceTests()
    {
        var key = nameof(TokenManagerServiceTests);

        _tokenManager = new(_mockUserRepository.Object, key);
    }

    [Fact]
    public async Task GenerateTokenAsync_ShouldReturnNull_WhenNoUserHasThatEmail()
    {
        // Arrange
        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        AuthRequest authRequest = new()
        {
            Email = "wrong@email.com",
            Password = "Password"
        };

        // Act
        var token = await _tokenManager.GenerateTokenAsync(authRequest);

        // Assert
        Assert.Null(token);
    }

    [Fact]
    public async Task GenerateTokenAsync_ShouldReturnNull_WhenPasswordIsWrong()
    {
        // Arrange
        var email = "correct@email.com";

        var user = User.CreateUser(email, "CorrectPassword", UserRole.Customer);

        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        AuthRequest authRequest = new()
        {
            Email = email,
            Password = "WrongPassword"
        };

        // Act
        var token = await _tokenManager.GenerateTokenAsync(authRequest);

        // Assert
        Assert.Null(token);
        Assert.Equal(user.Email, authRequest.Email);
        Assert.False(user.CheckPassword(authRequest.Password));
    }

    [Fact]
    public async Task GenerateTokenAsync_ShouldReturnToken_WhenEmailAndPasswordAreCorrect()
    {
        // Arrange
        var email = "correct@email.com";
        var password = "CorrectPassword";

        var user = User.CreateUser(email, password, UserRole.Customer);

        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        AuthRequest authRequest = new()
        {
            Email = email,
            Password = password
        };

        // Act
        var token = await _tokenManager.GenerateTokenAsync(authRequest);

        // Assert
        Assert.NotNull(token);
        Assert.Equal(user.Email, authRequest.Email);
        Assert.True(user.CheckPassword(authRequest.Password));
    }
}
