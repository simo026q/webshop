using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Api.Auth;
using Webshop.Api.Controllers;
using Webshop.Api.Services;
using Xunit;

namespace Webshop.Tests.Controllers;

public class AuthControllerTests
{
    private readonly AuthController _authController;
    private readonly Mock<ITokenManagerService> _mockTokenManager;

    public AuthControllerTests()
    {
        _mockTokenManager = new();
        
        _authController = new AuthController(_mockTokenManager.Object);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnOk_WhenEmailOrPasswordIsIncorrect()
    {
        // Arrange
        AuthRequest authRequest = new()
        {
            Email = "wrong@email.com",
            Password = "WrongPassword"
        };
        
        _mockTokenManager
            .Setup(x => x.GenerateTokenAsync(It.IsAny<AuthRequest>()))
            .ReturnsAsync((string?)null);

        // Act
        var response = await _authController.Authenticate(authRequest);
        
        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<AuthResponse>(objResult?.Value);

        var authResponse = (AuthResponse?)objResult?.Value;

        Assert.NotNull(authResponse);
        Assert.Null(authResponse?.Token);
        Assert.False(authResponse?.Success);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnOk_WhenEmailOrPasswordIsCorrect()
    {
        // Arrange
        AuthRequest authRequest = new()
        {
            Email = "correct@email.com",
            Password = "CorrectPassword"
        };

        var token = Guid.NewGuid().ToString();

        _mockTokenManager
            .Setup(x => x.GenerateTokenAsync(It.IsAny<AuthRequest>()))
            .ReturnsAsync((string?)token);

        // Act
        var response = await _authController.Authenticate(authRequest);

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<AuthResponse>(objResult?.Value);

        var authResponse = (AuthResponse?)objResult?.Value;

        Assert.NotNull(authResponse);
        Assert.NotNull(authResponse?.Token);
        Assert.Equal(token, authResponse?.Token);
        Assert.True(authResponse?.Success);
    }
}
