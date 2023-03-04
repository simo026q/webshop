using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Webshop.Api.Controllers;
using Webshop.Api.Dtos;
using Webshop.Api.Entities;
using Webshop.Api.Repositories;
using Webshop.Api.Services;
using Xunit;

namespace Webshop.Tests.Controllers;

public class AccountControllerTests
{
    private readonly AccountController _accountController;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Guid _userId;

    public AccountControllerTests()
    {
        _userId = Guid.NewGuid();

        _mockUserRepository = new();

        _accountController = new(_mockUserRepository.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>(1) { new Claim("id", _userId.ToString()) }))
                }
            }
        };
    }

    [Fact]
    public async Task GetUser_ShouldReturnStatusCode200_WhenUserIsValid()
    {
        // Arrange
        User user = new() { Email = "example@mail.com" };

        _mockUserRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var response = await _accountController.GetUser();

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<UserResponse>(objResult?.Value);

        var resultUser = (UserResponse?)objResult?.Value;

        Assert.NotNull(resultUser);
        Assert.Equal(user.Email, resultUser?.Email);
    }

    [Fact]
    public async Task GetUser_ShouldReturnStatusCode404_WhenUserDoesNotExsistInRepository()
    {
        // Arrange
        _mockUserRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);

        // Act
        var response = await _accountController.GetUser();

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);

        var objResult = (NotFoundResult?)response.Result;

        Assert.Equal(404, objResult?.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnStatusCode200_WhenUpdateSucceeded()
    {
        // Arrange
        UserUpdateRequest updateRequest = new()
        {
            Id = _userId,
            FullName = "New FullName",
            IsActive = true
        };

        User user = User.CreateUser("example@mail.com", "CorrectPassw0rd", UserRole.Customer, "Old FullName", _userId);

        User newUser = updateRequest.ToEntity(user);

        _mockUserRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        _mockUserRepository
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(newUser);

        // Act
        var response = await _accountController.UpdateUser(updateRequest);

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<UserResponse>(objResult?.Value);

        var resultUser = (UserResponse?)objResult?.Value;

        Assert.NotNull(resultUser);
        Assert.Equal(user.Email, resultUser?.Email);
        Assert.Equal("New FullName", resultUser?.FullName);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnStatusCode404_WhenUserDoesNotExsistInRepository()
    {
        // Arrange
        UserUpdateRequest updateRequest = new()
        {
            Id = _userId,
            FullName = "New FullName",
            IsActive = true
        };

        _mockUserRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);

        // Act
        var response = await _accountController.UpdateUser(updateRequest);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);

        var objResult = (NotFoundResult?)response.Result;

        Assert.Equal(404, objResult?.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnStatusCode403_WhenCurrectUserDoesNotMatchUserInRequest()
    {
        // Arrange
        UserUpdateRequest updateRequest = new()
        {
            Id = Guid.NewGuid(),
            FullName = "New FullName",
            IsActive = true
        };

        User user = User.CreateUser("example@mail.com", "CorrectPassw0rd", UserRole.Customer, "Old FullName", _userId);

        _mockUserRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var response = await _accountController.UpdateUser(updateRequest);

        // Assert
        Assert.IsType<ForbidResult>(response.Result);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnStatusCode500_WhenUpdateAsyncThrowsAnException()
    {
        // Arrange
        UserUpdateRequest updateRequest = new()
        {
            Id = _userId,
            FullName = "New FullName",
            IsActive = true
        };

        User user = User.CreateUser("example@mail.com", "CorrectPassw0rd", UserRole.Customer, "Old FullName", _userId);

        _mockUserRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        _mockUserRepository
            .Setup(x => x.UpdateAsync(user))
            .ThrowsAsync(new Exception());

        // Act
        var response = await _accountController.UpdateUser(updateRequest);

        // Assert
        Assert.IsType<ObjectResult>(response.Result);

        var objResult = (ObjectResult?)response.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnStatusCode200_WhenTheUserIsDeleted()
    {
        // Arrange
        User user = User.CreateUser("example@mail.com", "CorrectPassw0rd", UserRole.Customer, "Old FullName", _userId);

        _mockUserRepository
            .Setup(x => x.DeleteAsync(_userId))
            .ReturnsAsync(user);

        // Act
        var response = await _accountController.DeleteUser();

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;

        Assert.Equal(200, objResult?.StatusCode);

        var resultUser = (UserResponse?)objResult?.Value;

        Assert.NotNull(resultUser);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnStatusCode404_WhenUserDoesNotExsistInRepository()
    {
        // Arrange
        _mockUserRepository
            .Setup(x => x.DeleteAsync(_userId))
            .ReturnsAsync((User?)null);

        // Act
        var response = await _accountController.DeleteUser();

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);

        var objResult = (NotFoundResult?)response.Result;

        Assert.Equal(404, objResult?.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnStatusCode500_WhenDeleteAsyncThrowsAnException()
    {
        // Arrange
        _mockUserRepository
            .Setup(x => x.DeleteAsync(_userId))
            .ThrowsAsync(new Exception());

        // Act
        var response = await _accountController.DeleteUser();

        // Assert
        Assert.IsType<ObjectResult>(response.Result);

        var objResult = (ObjectResult?)response.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

    [Fact]
    public async Task ChangePassword_ShouldReturnStatusCode204_WhenPasswordIsUpdate()
    {
        // Arrange
        var currentPassword = "CorrectPassw0rd";

        ChangePasswordRequest passwordRequest = new()
        {
            Password = currentPassword,
            NewPassword = "NewPassw0rd"
        };

        var user = User.CreateUser("example@mail.com", currentPassword, UserRole.Customer, null, _userId);

        _mockUserRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        _mockUserRepository
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(new User());

        // Act
        var response = await _accountController.ChangePassword(passwordRequest);

        // Assert
        Assert.IsType<NoContentResult>(response);

        var objResult = (NoContentResult?)response;

        Assert.Equal(204, objResult?.StatusCode);
    }

    [Fact]
    public async Task ChangePassword_ShouldReturnStatusCode404_WhenUserDoesNotExsistInRepository()
    {
        // Arrange
        ChangePasswordRequest passwordRequest = new()
        {
            Password = "Passw0rd",
            NewPassword = "NewPassw0rd"
        };

        _mockUserRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);

        // Act
        var response = await _accountController.ChangePassword(passwordRequest);

        // Assert
        Assert.IsType<NotFoundResult>(response);

        var objResult = (NotFoundResult?)response;

        Assert.Equal(404, objResult?.StatusCode);
    }

    [Fact]
    public async Task ChangePassword_ShouldReturnStatusCode403_WhenPasswordInRequestIsInvalid()
    {
        // Arrange
        var currentPassword = "CorrectPassw0rd";

        ChangePasswordRequest passwordRequest = new()
        {
            Password = "InvalidPassw0rd",
            NewPassword = "NewPassw0rd"
        };

        var user = User.CreateUser("example@mail.com", currentPassword, UserRole.Customer, null, _userId);

        _mockUserRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var response = await _accountController.ChangePassword(passwordRequest);

        // Assert
        Assert.IsType<ForbidResult>(response);
    }

    [Fact]
    public async Task RegisterCustomer_ShouldReturnStatusCode201_WhenAUserIsCreated()
    {
        // Arrange
        RegisterCustomerRequest registerCustomer = new()
        {
            Email = "new@email.com",
            Password = "NewPassw0rd"
        };

        var user = registerCustomer.ToEntity();

        _mockUserRepository
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        // Act
        var response = await _accountController.RegisterCustomer(registerCustomer);

        // Assert
        Assert.IsType<CreatedAtActionResult>(response.Result);

        var objResult = (CreatedAtActionResult?)response.Result;

        Assert.Equal(201, objResult?.StatusCode);

        var resultUser = (UserResponse?)objResult?.Value;

        Assert.NotNull(resultUser);
        Assert.Equal(user.Email, resultUser?.Email);
    }

    [Fact]
    public async Task RegisterCustomer_ShouldReturnStatusCode500_WhenCreateAsyncThrowsAnException()
    {
        // Arrange
        RegisterCustomerRequest registerCustomer = new()
        {
            Email = "new@email.com",
            Password = "NewPassw0rd"
        };

        _mockUserRepository
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await _accountController.RegisterCustomer(registerCustomer);

        // Assert
        Assert.IsType<ObjectResult>(response.Result);

        var objResult = (ObjectResult?)response.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }
}
