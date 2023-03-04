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

public class OrdersControllerTests
{
    private readonly OrdersController _ordersController;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;

    public OrdersControllerTests()
    {
        _mockOrderRepository = new();
        
        _mockUserRepository = new();
        
        _mockProductRepository = new();
        
        _ordersController = new(_mockOrderRepository.Object, _mockProductRepository.Object, _mockUserRepository.Object)
        {
            ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext()
            }
        };

    }

    [Fact]
    public async Task GetOrders_ShouldReturnStatusCode200_WhenOrdersExists()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order
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
                    ZipCode = "1234AB"
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
            }
        };

        _mockOrderRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(orders);

        // Act
        var result = await _ordersController.GetAllOrders();

        // Assert
        var objResult = (OkObjectResult?)result.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<List<OrderResponse>>(objResult?.Value);

        var ordersResult = (List<OrderResponse>?)objResult?.Value;
        
        Assert.NotNull(ordersResult);
        Assert.NotEmpty(ordersResult);
    }

    [Fact]
    public async Task GetOrders_ShouldReturnStatusCode204_WhenNoOrdersExists()
    {
        // Arrange
        var orders = new List<Order>();

        _mockOrderRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(orders);

        // Act
        var result = await _ordersController.GetAllOrders();

        // Assert
        var objResult = (NoContentResult?)result.Result;

        Assert.Equal(204, objResult?.StatusCode);
    }

    [Fact]
    public async Task GetOrder_ShouldReturnStatusCode200_WhenOrderExists()
    {
        // Arrange
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
                ZipCode = "1234AB"
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

        _mockOrderRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(order);

        // Act
        var result = await _ordersController.GetOrder(order.Id);

        // Assert
        var objResult = (OkObjectResult?)result.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<OrderResponse>(objResult?.Value);

        var orderResult = (OrderResponse?)objResult?.Value;

        Assert.NotNull(orderResult);
        Assert.Equal(order.Id, orderResult?.Id);
    }

    [Fact]
    public async Task GetOrder_ShouldReturnStatusCode404_WhenOrderDoesNotExist()
    {
        // Arrange
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
                ZipCode = "1234AB"
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

        _mockOrderRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Order?)null);

        // Act
        var result = await _ordersController.GetOrder(order.Id);

        // Assert
        var objResult = (NotFoundResult?)result.Result;

        Assert.Equal(404, objResult?.StatusCode);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnStatusCode201_WhenOrderIsCreated()
    {
        // Arrange
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
                ZipCode = "1234AB"
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

        var orderRequest = new OrderRequest
        {
            Address = new AddressRequest
            {
                StreetName = order.Address.StreetName,
                StreetNumber = order.Address.StreetNumber,
                City = order.Address.City,
                ZipCode = order.Address.ZipCode
            },
            Id = order.Id,
            UserId = order.UserId,
            Products = order.Products.Select(x => new OrderProductRequest
            {
                ProductVariantId = x.ProductVariantId,
                Quantity = x.Quantity
            }).ToList()
        };

        _mockOrderRepository.Setup(x => x.CreateAsync(It.IsAny<Order>())).ReturnsAsync(order);

        // Act
        var result = await _ordersController.CreateOrder(orderRequest);

        // Assert
        var objResult = (CreatedAtActionResult?)result.Result;

        Assert.Equal(201, objResult?.StatusCode);
        Assert.IsType<OrderResponse>(objResult?.Value);

        var orderResult = (OrderResponse?)objResult?.Value;

        Assert.NotNull(orderResult);
        Assert.Equal(order.Id, orderResult?.Id);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnStatusCode500_WhenRepositoryThrowsException()
    {
        // Arrange
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
                ZipCode = "1234AB"
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

        var orderRequest = new OrderRequest
        {
            Address = new AddressRequest
            {
                StreetName = order.Address.StreetName,
                StreetNumber = order.Address.StreetNumber,
                City = order.Address.City,
                ZipCode = order.Address.ZipCode
            },
            Id = order.Id,
            UserId = order.UserId,
            Products = order.Products.Select(x => new OrderProductRequest
            {
                ProductVariantId = x.ProductVariantId,
                Quantity = x.Quantity
            }).ToList()
        };

        _mockOrderRepository.Setup(x => x.CreateAsync(It.IsAny<Order>())).ThrowsAsync(new Exception());

        // Act
        var result = await _ordersController.CreateOrder(orderRequest);

        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        
        var objResult = (ObjectResult?)result.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnStatusCode500_WhenOrderRequestIsNull()
    {
        // Arrange
        OrderRequest orderRequest = null;

        _mockOrderRepository.Setup(x => x.CreateAsync(It.IsAny<Order>()));

        // Act
        var result = await _ordersController.CreateOrder(orderRequest);

        // Assert
        Assert.IsType<ObjectResult>(result.Result);

        var objResult = (ObjectResult?)result.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

    [Fact]
    public async Task UpdateOrderStatus_ShouldReturnStatusCode404_WhenOrderIsNotFound()
    {
        // Arrange
        var request = new OrderStatusRequest();

        // Act
        var repsonse = await _ordersController.UpdateOrderStatus(Guid.NewGuid(), request);

        // Assert
        Assert.IsType<NotFoundResult>(repsonse.Result);

        var objResult = (NotFoundResult?)repsonse.Result;

        Assert.Equal(404, objResult?.StatusCode);
    }

    [Fact]
    public async Task UpdateOrderStatus_ShouldReturnStatusCode500_WhenOrderIdIsInvalid()
    {
        // Arrange
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
                ZipCode = "1234AB"
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

        var request = new OrderStatusRequest
        {
            Status = OrderStatus.Processing
        };

        _mockOrderRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(order);

        // Act
        var repsonse = await _ordersController.UpdateOrderStatus(order.Id, request);

        // Assert
        Assert.IsType<ObjectResult>(repsonse.Result);

        var objResult = (ObjectResult?)repsonse.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

    [Fact]
    public async Task GetOrderByUser_ShouldReturnStatusCode500_WhenUserIsNotAuthenticated()
    {
        // Arrange

        // Act
        var result = await _ordersController.GetOrderByUser(Guid.NewGuid());

        // Assert
        Assert.IsType<ObjectResult>(result.Result);

        var objResult = (ObjectResult?)result.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

    [Fact]
    public async Task GetAllOrdersByUser_ShouldReturnStatusCode500_WhenUserIsNotAuthenticated()
    {
        // Arrange

        // Act
        var result = await _ordersController.GetAllOrdersByUser();

        // Assert
        Assert.IsType<ObjectResult>(result.Result);

        var objResult = (ObjectResult?)result.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

}

