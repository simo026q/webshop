using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Api.Contexts;
using Webshop.Api.Entities;
using Webshop.Api.Repositories;
using Xunit;

namespace Webshop.Tests.Repositories;

public class CategoryRepositoryTests
{
    private readonly DbContextOptions<WebshopContext> _options;
    private readonly WebshopContext _context;
    private readonly CategoryRepository _categoryRepository;

    public CategoryRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<WebshopContext>()
            .UseInMemoryDatabase(databaseName: nameof(CategoryRepositoryTests))
            .Options;

        _context = new(_options);

        _categoryRepository = new(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAEmptyListOfCategories_WhenNocategoryExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var categories = await _categoryRepository.GetAllAsync();

        // Assert
        Assert.NotNull(categories);
        Assert.Empty(categories);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAListofCategories_WhenCategoriesExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        await _context.Categories.AddAsync(new() { Id = "category 1" });
        await _context.Categories.AddAsync(new() { Id = "category 2" });

        await _context.SaveChangesAsync();

        // Act
        var categories = await _categoryRepository.GetAllAsync();

        // Assert
        Assert.NotNull(categories);
        Assert.NotEmpty(categories);
        Assert.Equal(2, categories.Count);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var category = await _categoryRepository.GetAsync(string.Empty);

        // Assert
        Assert.Null(category);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var categoryId = "category 1";

        await _context.Categories.AddAsync(new() { Id = categoryId });

        await _context.SaveChangesAsync();

        // Act
        var category = await _categoryRepository.GetAsync(categoryId);

        // Assert
        Assert.NotNull(category);
        Assert.Equal(categoryId, category?.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCategory_WhenCategoryDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var categoryId = "category 1";

        // Act
        var category = await _categoryRepository.CreateAsync(new() { Id = categoryId });

        // Assert
        Assert.NotNull(category);
        Assert.Equal(categoryId, category.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotAddCategory_WhenCategoryExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var categoryId = "category 1";

        await _context.Categories.AddAsync(new() { Id = categoryId });

        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _categoryRepository.CreateAsync(new() { Id = categoryId }));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCategory_WhenCategoryExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var categoryId = "category 1";

        Category category = new() { Id = categoryId };

        await _context.Categories.AddAsync(category);

        await _context.SaveChangesAsync();

        // Act
        var item = await _categoryRepository.DeleteAsync(categoryId);

        // Assert
        Assert.Empty(await _context.Categories.ToListAsync());
        Assert.NotNull(item);
        Assert.Equal(categoryId, item?.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNull_WhencategoryDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var item = await _categoryRepository.DeleteAsync(string.Empty);

        // Assert
        Assert.Null(item);
    }

    [Fact]
    public async Task GetAllWithProductsAsync_ShouldReturnAListOfCateogries_WhenCategoriesWithProductsExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var category = await _context.Categories.AddAsync(new() { Id = "category 1" });
        await _context.Categories.AddAsync(new() { Id = "category 2" });

        var product = await _context.Products.AddAsync(new Product());

        await _context.ProductCategories.AddAsync(new ProductCategory { Category = category.Entity, Product = product.Entity });

        await _context.SaveChangesAsync();

        // Act
        var categories = await _categoryRepository.GetAllWithProductsAsync();

        // Assert
        Assert.NotNull(categories);
        Assert.NotEmpty(categories);
        Assert.Equal(1, categories?.Count);
    }

    [Fact]
    public async Task GetAllWithProductsAsync_ShouldReturnAEmptyListOfCategories_WhenCategoriesWithProductsDoNotExsist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        await _context.Categories.AddAsync(new() { Id = "category 1" });
        await _context.Categories.AddAsync(new() { Id = "category 2" });

        await _context.SaveChangesAsync();

        // Act
        var categories = await _categoryRepository.GetAllWithProductsAsync();

        // Assert
        Assert.NotNull(categories);
        Assert.Empty(categories);
    }
}
