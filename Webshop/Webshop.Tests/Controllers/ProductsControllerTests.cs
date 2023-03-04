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

public class ProductsControllerTests
{
    private readonly ProductsController _productsController;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;

    public ProductsControllerTests()
    {
        _mockProductRepository = new();

        _mockCategoryRepository = new();

        _productsController = new(_mockProductRepository.Object, _mockCategoryRepository.Object)
        {
            // To add the response headers
            ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Theory]
    [InlineData(1, 1, 10, 1)]
    [InlineData(10, 1, 10, 10)]
    [InlineData(11, 1, 10, 10)]
    [InlineData(11, 2, 10, 1)]
    public async Task GetAllProducts_ShouldReturnStatusCode200_WhenProductsExsistInPage(int totalProducts, int page, int pageSize, int expectedItems)
    {
        // Arrange
        var products = new List<Product>();

        for (int i = 0; i < totalProducts; i++)
        {
            products.Add(new()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Description = "Test",
                CreatedAt = DateTime.Now
            });
        }

        _mockProductRepository
            .Setup(r => r.GetByCategoryAndSearchAsync(null, null))
            .ReturnsAsync(products);

        // Act
        var result = await _productsController.GetAllProducts(page: page, size: pageSize);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);

        var objResult = (OkObjectResult?)result.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<List<ProductResponse>>(objResult?.Value);

        var productsResult = (List<ProductResponse>?)objResult?.Value;

        Assert.NotNull(productsResult);
        Assert.NotEmpty(productsResult);
        Assert.Equal(expectedItems, productsResult?.Count);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnStatusCode204_WhenNoProductsExists()
    {
        // Arrange
        _mockProductRepository
            .Setup(r => r.GetByCategoryAndSearchAsync(null, null))
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _productsController.GetAllProducts();

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnStatusCode200_WhenProductsExsist()
    {
        // Arrange
        _mockProductRepository
            .Setup(r => r.GetByCategoryAndSearchAsync(null, null))
            .ReturnsAsync(new List<Product>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Description = "Test",
                    CreatedAt = DateTime.Now
                }
            });

        // Act
        var result = await _productsController.GetAllProducts();

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);

        var objResult = (OkObjectResult?)result.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<List<ProductResponse>>(objResult?.Value);
        
        var products = (List<ProductResponse>?)objResult?.Value;

        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.Equal(1, products?.Count);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnStatusCode200_WhenProductsExsistAndCategoryIsSet()
    {
        // Arrange
        _mockProductRepository
            .Setup(r => r.GetByCategoryAndSearchAsync(null, "Wheels"))
            .ReturnsAsync(new List<Product>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Description = "Test",
                    CreatedAt = DateTime.Now
                }
            });

        // Act
        var result = await _productsController.GetAllProducts("Wheels");

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);

        var objResult = (OkObjectResult?)result.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<List<ProductResponse>>(objResult?.Value);

        var products = (List<ProductResponse>?)objResult?.Value;

        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.Equal(1, products?.Count);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnStatusCode200_WhenProductsExsistAndSearchIsSet()
    {
        // Arrange
        _mockProductRepository
            .Setup(r => r.GetByCategoryAndSearchAsync("Test", null))
            .ReturnsAsync(new List<Product>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Description = "Test",
                    CreatedAt = DateTime.Now
                }
            });

        // Act
        var result = await _productsController.GetAllProducts(null, "Test");

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);

        var objResult = (OkObjectResult?)result.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<List<ProductResponse>>(objResult?.Value);

        var products = (List<ProductResponse>?)objResult?.Value;

        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.Equal(1, products?.Count);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnStatusCode400_WhenPageIsOutOfRange()
    {
        // Arrange
        _mockProductRepository
            .Setup(r => r.GetByCategoryAndSearchAsync(null, null))
            .ReturnsAsync(new List<Product>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Description = "Test",
                    CreatedAt = DateTime.Now
                }
            });

        // Act
        var result = await _productsController.GetAllProducts(page: 2, size: 1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);

        var objResult = (BadRequestObjectResult?)result.Result;

        Assert.Equal(400, objResult?.StatusCode);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnStatusCode200_WhenProductExists()
    {
        // Arrange
        var product = new Product() { Id = Guid.NewGuid(), Name = "Product 1" };

        _mockProductRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(product);

        // Act
        var response = await _productsController.GetProduct(Guid.NewGuid());

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<ProductResponse>(objResult?.Value);

        var result = (ProductResponse?)objResult?.Value;

        Assert.NotNull(result);
        Assert.Equal(product.Id, result?.Id);
        Assert.Equal(product.Name, result?.Name);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnStatusCode404_WhenProductDoesNotExist()
    {
        // Arrange
        _mockProductRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product?)null);

        // Act
        var response = await _productsController.GetProduct(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);

        var objResult = (NotFoundResult?)response.Result;
        
        Assert.Equal(404, objResult?.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnStatusCode201_WhenProductIsCreated()
    {
        // Arrange
        var productRequest = new ProductRequest() { Id = Guid.NewGuid(), Name = "Product 1" };
        
        var product = productRequest.ToEntity();

        _mockProductRepository
            .Setup(x => x.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(product);

        // Act
        var response = await _productsController.CreateProduct(productRequest);

        // Assert
        Assert.IsType<CreatedAtActionResult>(response.Result);

        var objResult = (CreatedAtActionResult?)response.Result;

        Assert.Equal(201, objResult?.StatusCode);
        Assert.IsType<ProductResponse>(objResult?.Value);

        var result = (ProductResponse?)objResult?.Value;

        Assert.NotNull(result);
        Assert.Equal(product.Id, result?.Id);
        Assert.Equal(product.Name, result?.Name);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnStatusCode500_WhenRepositoryThrowsException()
    {
        // Arrange
        var productRequest = new ProductRequest() { Id = Guid.NewGuid(), Name = "Product 1" };

        _mockProductRepository
            .Setup(x => x.CreateAsync(It.IsAny<Product>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await _productsController.CreateProduct(productRequest);

        // Assert
        Assert.IsType<ObjectResult>(response.Result);

        var objResult = (ObjectResult?)response.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnStatusCode200_WhenProductIsUpdated()
    {
        // Arrange
        var productRequest = new ProductRequest() { Id = Guid.NewGuid(), Name = "Product 1" };

        var product = productRequest.ToEntity();

        _mockProductRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync(product);

        // Act
        var response = await _productsController.UpdateProduct(productRequest.Id, productRequest);

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<ProductResponse>(objResult?.Value);

        var result = (ProductResponse?)objResult?.Value;

        Assert.NotNull(result);
        Assert.Equal(product.Id, result?.Id);
        Assert.Equal(product.Name, result?.Name);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnStatusCode400_WhenProductDoesNotExist()
    {
        // Arrange
        var productRequest = new ProductRequest() { Id = Guid.NewGuid(), Name = "Product 1" };

        var product = productRequest.ToEntity();

        _mockProductRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync(product);

        // Act
        var response = await _productsController.UpdateProduct(Guid.NewGuid(), productRequest);

        // Assert
        Assert.IsType<BadRequestResult>(response.Result);

        var objResult = (BadRequestResult?)response.Result;

        Assert.Equal(400, objResult?.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnStatusCode500_WhenRepositoryThrowsException()
    {
        // Arrange
        var productRequest = new ProductRequest() { Id = Guid.NewGuid(), Name = "Product 1" };

        _mockProductRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Product>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await _productsController.UpdateProduct(productRequest.Id, productRequest);

        // Assert
        Assert.IsType<ObjectResult>(response.Result);

        var objResult = (ObjectResult?)response.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

    [Fact]
    public async Task DisableProduct_ShouldReturnStatusCode404_WhenProductIsNotFound()
    {
        // Arrange
        _mockProductRepository
            .Setup(x => x.DisableAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product?)null);

        // Act
        var repsonse = await _productsController.DisableProduct(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(repsonse.Result);

        var objResult = (NotFoundResult?)repsonse.Result;

        Assert.Equal(404, objResult?.StatusCode);
    }

    [Fact]
    public async Task DisableProduct_ShouldReturnStatusCode200_WhenProductIsDisabled()
    {
        // Arrange
        var product = new Product() { Id = Guid.NewGuid(), Name = "Product 1" };

        _mockProductRepository
            .Setup(x => x.DisableAsync(It.IsAny<Guid>()))
            .ReturnsAsync(product);

        // Act
        var repsonse = await _productsController.DisableProduct(product.Id);

        // Assert
        Assert.IsType<OkObjectResult>(repsonse.Result);

        var objResult = (OkObjectResult?)repsonse.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<ProductResponse>(objResult?.Value);

        var result = (ProductResponse?)objResult?.Value;

        Assert.NotNull(result);
        Assert.Equal(product.Id, result?.Id);
        Assert.Equal(product.Name, result?.Name);
    }

    [Fact]
    public async Task DisableProduct_ShouldReturnStatusCode500_WhenRepositoryThrowsException()
    {
        // Arrange
        _mockProductRepository
            .Setup(x => x.DisableAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var repsonse = await _productsController.DisableProduct(Guid.NewGuid());

        // Assert
        Assert.IsType<ObjectResult>(repsonse.Result);

        var objResult = (ObjectResult?)repsonse.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

    [Fact]
    public async Task ActivateProduct_ShouldReturnStatusCode404_WhenProductIsNotFound()
    {
        // Arrange
        _mockProductRepository
            .Setup(x => x.ActivateAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product?)null);

        // Act
        var repsonse = await _productsController.ActivateProduct(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(repsonse.Result);

        var objResult = (NotFoundResult?)repsonse.Result;

        Assert.Equal(404, objResult?.StatusCode);
    }

    [Fact]
    public async Task ActivateProduct_ShouldReturnStatusCode200_WhenProductIsActivated()
    {
        // Arrange
        var product = new Product() { Id = Guid.NewGuid(), Name = "Product 1" };

        _mockProductRepository
            .Setup(x => x.ActivateAsync(It.IsAny<Guid>()))
            .ReturnsAsync(product);

        // Act
        var repsonse = await _productsController.ActivateProduct(product.Id);

        // Assert
        Assert.IsType<OkObjectResult>(repsonse.Result);

        var objResult = (OkObjectResult?)repsonse.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<ProductResponse>(objResult?.Value);

        var result = (ProductResponse?)objResult?.Value;

        Assert.NotNull(result);
        Assert.Equal(product.Id, result?.Id);
        Assert.Equal(product.Name, result?.Name);
    }

    [Fact]
    public async Task ActivateProduct_ShouldReturnStatusCode500_WhenRepositoryThrowsException()
    {
        // Arrange
        _mockProductRepository
            .Setup(x => x.ActivateAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var repsonse = await _productsController.ActivateProduct(Guid.NewGuid());

        // Assert
        Assert.IsType<ObjectResult>(repsonse.Result);

        var objResult = (ObjectResult?)repsonse.Result;

        Assert.Equal(500, objResult?.StatusCode);
    }

    [Fact]
    public async Task GetLatestProducts_ShouldReturnStatusCode200_WhenThereIsProducts()
    {
        // Arrange
        var products = new List<Product>()
        {
            new Product() { Id = Guid.NewGuid(), Name = "Product 1" },
            new Product() { Id = Guid.NewGuid(), Name = "Product 2" },
            new Product() { Id = Guid.NewGuid(), Name = "Product 3" },
        };

        _mockProductRepository
            .Setup(x => x.GetTopActiveAsync(It.IsAny<int>()))
            .ReturnsAsync(products);

        // Act
        var repsonse = await _productsController.GetLatestProducts(3);

        // Assert
        Assert.IsType<OkObjectResult>(repsonse.Result);

        var objResult = (OkObjectResult?)repsonse.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<List<ProductResponse>>(objResult?.Value);

        var result = (List<ProductResponse>?)objResult?.Value;

        Assert.NotNull(result);
        Assert.Equal(products.Count, result?.Count);

        for (int i = 0; i < products.Count; i++)
        {
            Assert.Equal(products[i].Id, result?[i].Id);
            Assert.Equal(products[i].Name, result?[i].Name);
        }
    }
}
