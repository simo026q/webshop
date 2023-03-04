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

public class CategoriesControllerTests
{
    private readonly CategoriesController _categoriesController;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;

    public CategoriesControllerTests()
    {
        _mockCategoryRepository = new();

        _categoriesController = new(_mockCategoryRepository.Object);
    }

    [Fact]
    public async Task GetAllCategoriesWithProduct_ShouldReturnStatusCode200_WhenThereIsNoCategories()
    {
        // Arrange
        _mockCategoryRepository
            .Setup(x => x.GetAllWithProductsAsync())
            .ReturnsAsync(new List<Category>());

        // Act
        var response = await _categoriesController.GetAllCategoriesWithProduct();

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<List<CategoryResponse>>(objResult?.Value);

        var categories = (List<CategoryResponse>?)objResult?.Value;

        Assert.NotNull(categories);
        Assert.Empty(categories);
    }

    [Fact]
    public async Task GetAllCategoriesWithProduct_ShouldReturnStatusCode200_WhenThereIsCategories()
    {
        // Arrange
        List<Category> categories = new()
        {
            new()
            {
                Id = "Wheels"
            },
            new()
            {
                Id = "Pedals"
            }
        };

        _mockCategoryRepository
            .Setup(x => x.GetAllWithProductsAsync())
            .ReturnsAsync(categories);

        // Act
        var response = await _categoriesController.GetAllCategoriesWithProduct();

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<List<CategoryResponse>>(objResult?.Value);

        var result = (List<CategoryResponse>?)objResult?.Value;

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result?.Count);
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturnStatusCode200_WhenThereIsNoCategories()
    {
        // Arrange
        _mockCategoryRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Category>());

        // Act
        var response = await _categoriesController.GetAllCategories();

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;
        
        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<List<CategoryResponse>>(objResult?.Value);

        var categories = (List<CategoryResponse>?)objResult?.Value;

        Assert.NotNull(categories);
        Assert.Empty(categories);
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturnStatusCode200_WhenThereIsCategories()
    {
        // Arrange
        List<Category> categories = new()
        {
            new()
            {
                Id = "Wheels"
            },
            new()
            {
                Id = "Pedals"
            }
        };
        
        _mockCategoryRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(categories);

        // Act
        var response = await _categoriesController.GetAllCategories();

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<List<CategoryResponse>>(objResult?.Value);

        var result = (List<CategoryResponse>?)objResult?.Value;

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result?.Count);
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnStatusCode201_WhenTheCategoryIsCreated()
    {
        // Arrange
        var categoryRequest = new CategoryRequest()
        {
            Id = "Wheels"
        };

        var category = categoryRequest.ToEntity();

        _mockCategoryRepository
            .Setup(x => x.CreateAsync(It.IsAny<Category>()))
            .ReturnsAsync(category);

        // Act
        var response = await _categoriesController.CreateCategory(categoryRequest);

        // Assert
        Assert.IsType<CreatedAtActionResult>(response.Result);

        var objResult = (CreatedAtActionResult?)response.Result;
        
        Assert.Equal(201, objResult?.StatusCode);
        Assert.IsType<CategoryResponse>(objResult?.Value);

        var result = (CategoryResponse?)objResult?.Value;

        Assert.NotNull(result);
        Assert.Equal("Wheels", result?.Id);
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturnStatusCode404_WhenCategoryDoesNotExsist()
    {
        // Arrange
        _mockCategoryRepository
            .Setup(x => x.DeleteAsync(It.IsAny<string>()))
            .ReturnsAsync((Category?)null);

        // Act
        var response = await _categoriesController.DeleteCategory("Wheels");

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);

        var objResult = (NotFoundResult?)response.Result;

        Assert.Equal(404, objResult?.StatusCode);
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturnStatusCode200_WhenCategoryIsDeleted()
    {
        // Arrange
        var category = new Category()
        {
            Id = "Wheels"
        };

        _mockCategoryRepository
            .Setup(x => x.DeleteAsync(It.IsAny<string>()))
            .ReturnsAsync(category);

        // Act
        var response = await _categoriesController.DeleteCategory("Wheels");

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);

        var objResult = (OkObjectResult?)response.Result;

        Assert.Equal(200, objResult?.StatusCode);
        Assert.IsType<CategoryResponse>(objResult?.Value);

        var result = (CategoryResponse?)objResult?.Value;

        Assert.NotNull(result);
    }
}
