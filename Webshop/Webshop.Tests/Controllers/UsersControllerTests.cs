using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Api.Controllers;
using Webshop.Api.Dtos;
using Webshop.Api.Entities;
using Webshop.Api.Repositories;
using Xunit;

namespace Webshop.Tests.Controllers;

public class UsersControllerTests
{
    private readonly UsersController _usersController;
    private readonly Mock<IUserRepository> _mockUserRepository;

    public UsersControllerTests()
    {
        _mockUserRepository = new();

        _usersController = new(_mockUserRepository.Object)
        {
            // To add the response headers
            ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnStatusCode204_WhenNoUsersExsist()
    {
        // Arrange
        var users = new List<User>();
        _mockUserRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _usersController.GetAllUsers();

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnStatusCode200_WhenUserExsist()
    {
        // Arrange
        var users = new List<User>
        {
            User.CreateUser("example@email.com", "CorrectPassw0rd", UserRole.Customer)
        };
        
        _mockUserRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _usersController.GetAllUsers();

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnStatusCode400_WhenPageIsOutOfRange()
    {
        // Arrange
        var users = new List<User>
        {
            User.CreateUser("example@email.com", "CorrectPassw0rd", UserRole.Customer)
        };
        
        _mockUserRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _usersController.GetAllUsers(2);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnStatusCode500_WhenExceptionIsThrown()
    {

        // Arrange
        var users = new List<User>
        {
            User.CreateUser("example@email.com", "CorrectPassw0rd", UserRole.Customer)
        };
        
        _mockUserRepository
            .Setup(x => x.GetAllAsync())
            .Throws(new Exception());

        // Act
        var result = await _usersController.GetAllUsers(1, 50);

        // Assert
        Assert.IsType<ObjectResult>(result.Result);

        var objResult = (ObjectResult?)result.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

    [Fact]
    public async Task GetUser_ShouldReturnStatusCode200_WhenUserExsist()
    {
        // Arrange
        var user = User.CreateUser("example@email.com", "CorrectPassw0rd", UserRole.Customer);

        _mockUserRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var result = await _usersController.GetUser(user.Id);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);

        var objResult = (OkObjectResult?)result.Result;

        Assert.Equal(200, objResult?.StatusCode);

        Assert.IsType<UserResponse>(objResult?.Value);

        var resultUser = (UserResponse?)objResult?.Value;

        Assert.NotNull(resultUser);
        Assert.Equal(user.Id, resultUser?.Id);
    }

    [Fact]
    public async Task GetUser_ShouldReturnStatusCode404_WhenUserDoNotExsist()
    {
        // Arrange
        var user = User.CreateUser("example@email.com", "WrongPassw0rd", UserRole.Customer);

        _mockUserRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _usersController.GetUser(user.Id);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);

        var objResult = (NotFoundResult?)result.Result;

        Assert.Equal(404, objResult?.StatusCode);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnStatusCode201_WhenUserIsCreated()
    {
        // Assert
        UserCreateRequest createRequest = new()
        {
            Id = Guid.Empty,
            Email = "example@mail.com",
            FullName = "",
            Role = UserRole.Customer
        };

        var user = createRequest.ToEntity();
        
        _mockUserRepository
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        // Act
        var result = await _usersController.CreateUser(createRequest);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result.Result);

        var objResult = (CreatedAtActionResult?)result.Result;

        Assert.Equal(201, objResult?.StatusCode);

        Assert.IsType<UserResponse>(objResult?.Value);

        var resultUser = (UserResponse?)objResult?.Value;

        Assert.NotNull(resultUser);
        Assert.Equal(user.Email, resultUser?.Email);
    }
}
