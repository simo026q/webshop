using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Threading.Tasks;
using Webshop.Api.Contexts;
using Webshop.Api.Entities;
using Webshop.Api.Repositories;
using Xunit;

namespace Webshop.Tests.Repositories;

public class UserRepositoryTests
{
    private readonly DbContextOptions<WebshopContext> _options;
    private readonly WebshopContext _context;
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<WebshopContext>()
            .UseInMemoryDatabase(databaseName: nameof(UserRepositoryTests))
            .Options;

        _context = new(_options);

        _userRepository = new(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAEmptyListOfUsers_WhenNoUserExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var users = await _userRepository.GetAllAsync();

        // Assert
        Assert.NotNull(users);
        Assert.Empty(users);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAListofUsers_WhenUsersExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        await _context.Users.AddAsync(new() { Id = Guid.NewGuid(), Email = "User 1" });
        await _context.Users.AddAsync(new() { Id = Guid.NewGuid(), Email = "User 2" });

        await _context.SaveChangesAsync();

        // Act
        var users = await _userRepository.GetAllAsync();

        // Assert
        Assert.NotNull(users);
        Assert.NotEmpty(users);
        Assert.Equal(2, users.Count);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var user = await _userRepository.GetAsync(Guid.NewGuid());

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var userId = Guid.NewGuid();

        await _context.Users.AddAsync(new() { Id = userId, Email = "User 1" });

        await _context.SaveChangesAsync();

        // Act
        var user = await _userRepository.GetAsync(userId);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(userId, user?.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddUser_WhenUserDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var userId = Guid.NewGuid();

        // Act
        var user = await _userRepository.CreateAsync(new() { Id = userId, Email = "User 1" });

        // Assert
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotAddUser_WhenUserExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var userId = Guid.NewGuid();

        await _context.Users.AddAsync(new() { Id = userId, Email = "User 1" });

        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRepository.CreateAsync(new() { Id = userId, Email = "User 1" }));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_WhenUserExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var userId = Guid.NewGuid();

        User user = new() { Id = userId, Email = "User 1" };

        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();

        // Act
        user.Email = "User 2";

        var updatedUser = await _userRepository.UpdateAsync(user);

        // Assert
        Assert.NotNull(updatedUser);
        Assert.Equal(userId, updatedUser.Id);
        Assert.Equal("User 2", updatedUser.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowAException_WhenUserDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await _userRepository.UpdateAsync(new() { Id = Guid.NewGuid(), Email = "User 1" }));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var userId = Guid.NewGuid();

        User user = new() { Id = userId, Email = "User 1" };

        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();

        // Act
        var item = await _userRepository.DeleteAsync(userId);

        // Assert
        Assert.Empty(await _context.Users.ToListAsync());
        Assert.NotNull(item);
        Assert.Equal(userId, item?.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var item = await _userRepository.DeleteAsync(Guid.NewGuid());

        // Assert
        Assert.Null(item);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenAUserWithThatEmailDoesNotExist() 
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var user = await _userRepository.GetByEmailAsync("User 1");

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenAUserWithThatEmailExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var userId = Guid.NewGuid();

        await _context.Users.AddAsync(new() { Id = userId, Email = "User 1" });

        await _context.SaveChangesAsync();

        // Act
        var user = await _userRepository.GetByEmailAsync("User 1");

        // Assert
        Assert.NotNull(user);
        Assert.Equal(userId, user?.Id);
    }
}
