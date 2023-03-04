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

public class ProductRepositoryTests
{
    private readonly DbContextOptions<WebshopContext> _options;
    private readonly WebshopContext _context;
    private readonly ProductRepository _productRepository;

    public ProductRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<WebshopContext>()
            .UseInMemoryDatabase(databaseName: nameof(ProductRepositoryTests))
            .Options;

        _context = new(_options);

        _productRepository = new(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAEmptyListOfProducts_WhenNoProductExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var products = await _productRepository.GetAllAsync();

        // Assert
        Assert.NotNull(products);
        Assert.Empty(products);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAListofProducts_WhenProductsExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        await _context.Products.AddAsync(new() { Id = Guid.NewGuid(), Name = "Product 1" });
        await _context.Products.AddAsync(new() { Id = Guid.NewGuid(), Name = "Product 2" });

        await _context.SaveChangesAsync();

        // Act
        var products = await _productRepository.GetAllAsync();

        // Assert
        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.Equal(2, products.Count);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var product = await _productRepository.GetAsync(Guid.NewGuid());

        // Assert
        Assert.Null(product);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnAProductWithNavigationalProperties_WhenProductExsist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var categoryId = "Category";
        
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 1",
            Categories = new List<ProductCategory>
            {
                new()
                {
                    CategoryId = categoryId,
                    Category = new Category
                    {
                        Id = categoryId
                    }
                }
            }
        };

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetAsync(product.Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result?.Categories);
        Assert.NotEmpty(result?.Categories);
        Assert.NotNull(result?.Categories.First().Category);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var productId = Guid.NewGuid();

        await _context.Products.AddAsync(new() { Id = productId, Name = "Product 1" });

        await _context.SaveChangesAsync();

        // Act
        var product = await _productRepository.GetAsync(productId);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(productId, product?.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProduct_WhenProductDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var productId = Guid.NewGuid();

        // Act
        var product = await _productRepository.CreateAsync(new() { Id = productId, Name = "Product 1" });

        // Assert
        Assert.NotNull(product);
        Assert.Equal(productId, product.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProductAndCategory_WhenProductDoesNotExsist() 
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var categoryId = "Category";

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 1",
            Categories = new List<ProductCategory>
            {
                new()
                {
                    CategoryId = categoryId,
                    Category = new Category
                    {
                        Id = categoryId
                    }
                }
            }
        };

        // Act
        var createdProduct = await _productRepository.CreateAsync(product);

        // Assert
        Assert.NotNull(createdProduct);

        Assert.NotNull(createdProduct?.Categories);
        Assert.NotEmpty(createdProduct?.Categories);
        Assert.NotNull(createdProduct?.Categories.First().Category);

        Assert.Equal(product.Name, createdProduct?.Name);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotAddProduct_WhenProductExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var productId = Guid.NewGuid();

        await _context.Products.AddAsync(new() { Id = productId, Name = "Product 1" });

        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _productRepository.CreateAsync(new() { Id = productId, Name = "Product 1" }));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var productId = Guid.NewGuid();

        Product product = new() { Id = productId, Name = "Product 1" };

        await _context.Products.AddAsync(product);

        await _context.SaveChangesAsync();

        // Act
        product.Name = "Product 2";

        var updatedProduct = await _productRepository.UpdateAsync(product);

        // Assert
        Assert.NotNull(updatedProduct);
        Assert.Equal(productId, updatedProduct.Id);
        Assert.Equal("Product 2", updatedProduct.Name);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteProduct_WhenProductExists()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var productId = Guid.NewGuid();

        Product product = new() { Id = productId, Name = "Product 1" };

        await _context.Products.AddAsync(product);

        await _context.SaveChangesAsync();

        // Act
        var item = await _productRepository.DeleteAsync(productId);

        // Assert
        Assert.Empty(await _context.Products.ToListAsync());
        Assert.NotNull(item);
        Assert.Equal(productId, item?.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteProductAndProductCategory_WhenProductExsist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var categoryId = "Category";

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 1",
            Categories = new List<ProductCategory>
            {
                new()
                {
                    CategoryId = categoryId,
                    Category = new Category
                    {
                        Id = categoryId
                    }
                }
            }
        };

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var item = await _productRepository.DeleteAsync(product.Id);

        // Assert
        Assert.NotNull(item);
        
        Assert.Equal(product.Id, item?.Id);
        
        Assert.Empty(await _context.Products.ToListAsync());
        Assert.Empty(await _context.ProductCategories.ToListAsync());
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        // Act
        var item = await _productRepository.DeleteAsync(Guid.NewGuid());

        // Assert
        Assert.Null(item);
    }

    [Fact]
    public async Task GetTopActiveAsync_ShouldReturnAListOfProducts_WhenThereIsActiveProducts()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();

        var productId = Guid.NewGuid();

        await _context.Products.AddAsync(new() { Id = productId, Name = "Product 1" });

        await _context.SaveChangesAsync();

        // Act
        var products = await _productRepository.GetTopActiveAsync(1);

        // Assert
        Assert.NotNull(products);
        Assert.NotEmpty(products);
    }
}
