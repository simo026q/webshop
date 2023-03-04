using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Api.Contexts;
using Webshop.Api.Entities;
using Webshop.Api.Repositories;
using Xunit;

namespace Webshop.Tests.Repositories;

public class OrderRepositoryTests
{
    private readonly DbContextOptions<WebshopContext> _options;
    private readonly WebshopContext _context;
    private readonly OrderRepository _orderRepository;

    public OrderRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<WebshopContext>()
            .UseInMemoryDatabase(databaseName: nameof(ProductRepositoryTests))
            .Options;

        _context = new(_options);

        _orderRepository = new(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAEmptyListOfOrders_WhenNoOrderExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var orders = await _orderRepository.GetAllAsync();

        // Assert
        Assert.NotNull(orders);
        Assert.Empty(orders);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var order = await _orderRepository.GetAsync(Guid.NewGuid());

        // Assert
        Assert.Null(order);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateOrder_WhenOrderDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Address = new Address
            {
                Id = Guid.NewGuid(),
                StreetName = "Teststreet",
                StreetNumber = "1",
                City = "Testcity",
                ZipCode = "1234AB",
                Country = "Testcountry"
            },
            AddressId = Guid.NewGuid(),
            Products = new List<OrderProduct>
                {
                    new OrderProduct
                    {
                        Price = 10,
                        OrderId = Guid.NewGuid(),
                        ProductVariantId = Guid.NewGuid(),
                        ProductVariant = new ProductVariant
                        {
                            Id = Guid.NewGuid(),
                            ProductId = Guid.NewGuid(),
                            Name = "Testvariant",
                            Description = "Testdescription",
                            SellingPrice = 10,
                            PurchasePrice = 5,
                            IsActive = true,
                            Offers = new List<ProductOffer>(),
                            Stock = 10
                        },
                        Quantity = 1
                    }
                }
        };

        // Act
        await _orderRepository.CreateAsync(order);

        // Assert
        var result = await _context.Orders.FirstOrDefaultAsync(x => x.Id == order.Id);

        Assert.NotNull(result);
        Assert.Equal(order.Id, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateOrder_WhenOrderExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Address = new Address
            {
                Id = Guid.NewGuid(),
                StreetName = "Teststreet",
                StreetNumber = "1",
                City = "Testcity",
                ZipCode = "1234AB",
                Country = "Testcountry"
            },
            AddressId = Guid.NewGuid(),
            Products = new List<OrderProduct>
                {
                    new OrderProduct
                    {
                        Price = 10,
                        OrderId = Guid.NewGuid(),
                        ProductVariantId = Guid.NewGuid(),
                        ProductVariant = new ProductVariant
                        {
                            Id = Guid.NewGuid(),
                            ProductId = Guid.NewGuid(),
                            Name = "Testvariant",
                            Description = "Testdescription",
                            SellingPrice = 10,
                            PurchasePrice = 5,
                            IsActive = true,
                            Offers = new List<ProductOffer>(),
                            Stock = 10
                        },
                        Quantity = 1
                    }
                }
        };

        await _context.Orders.AddAsync(order);

        await _context.SaveChangesAsync();

        // Act
        order.Status = OrderStatus.Processing;

        await _orderRepository.UpdateAsync(order);

        // Assert
        var result = await _context.Orders.FirstOrDefaultAsync(x => x.Id == order.Id);

        Assert.NotNull(result);
        Assert.Equal(order.Id, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenOrderDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Address = new Address
            {
                Id = Guid.NewGuid(),
                StreetName = "Teststreet",
                StreetNumber = "1",
                City = "Testcity",
                ZipCode = "1234AB",
                Country = "Testcountry"
            },
            AddressId = Guid.NewGuid(),
            Products = new List<OrderProduct>
                {
                    new OrderProduct
                    {
                        Price = 10,
                        OrderId = Guid.NewGuid(),
                        ProductVariantId = Guid.NewGuid(),
                        ProductVariant = new ProductVariant
                        {
                            Id = Guid.NewGuid(),
                            ProductId = Guid.NewGuid(),
                            Name = "Testvariant",
                            Description = "Testdescription",
                            SellingPrice = 10,
                            PurchasePrice = 5,
                            IsActive = true,
                            Offers = new List<ProductOffer>(),
                            Stock = 10
                        },
                        Quantity = 1
                    }
                }
        };

        // Act
        var exception = await Record.ExceptionAsync(() => _orderRepository.UpdateAsync(order));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteOrder_WhenOrderExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Address = new Address
            {
                Id = Guid.NewGuid(),
                StreetName = "Teststreet",
                StreetNumber = "1",
                City = "Testcity",
                ZipCode = "1234AB",
                Country = "Testcountry"
            },
            AddressId = Guid.NewGuid(),
            Products = new List<OrderProduct>
                {
                    new OrderProduct
                    {
                        Price = 10,
                        OrderId = Guid.NewGuid(),
                        ProductVariantId = Guid.NewGuid(),
                        ProductVariant = new ProductVariant
                        {
                            Id = Guid.NewGuid(),
                            ProductId = Guid.NewGuid(),
                            Name = "Testvariant",
                            Description = "Testdescription",
                            SellingPrice = 10,
                            PurchasePrice = 5,
                            IsActive = true,
                            Offers = new List<ProductOffer>(),
                            Stock = 10
                        },
                        Quantity = 1
                    }
                }
        };

        await _context.Orders.AddAsync(order);
        
        await _context.SaveChangesAsync();

        // Act
        await _orderRepository.DeleteAsync(order.Id);

        // Assert
        var result = await _context.Orders.FirstOrDefaultAsync(x => x.Id == order.Id);

        Assert.Null(result);
    }

}
