using Webshop.Api.Interfaces;
using Webshop.Api.Entities;
using Webshop.Api.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Webshop.Api.Repositories;

public class ProductRepository : RepositoryBase<Guid, Product>, IProductRepository
{
    private readonly WebshopContext _context;

    public ProductRepository(WebshopContext context) : base(context)
    {
        _context = context;
    }

    private IQueryable<Product> GetProductsWithRelations()
    {
        return _context.Products
            .Include(p => p.Variants)
            .Include(p => p.Categories)
            .ThenInclude(pc => pc.Category);
    }

    private async Task<List<ProductCategory>> GetTrackedCategoriesAsync(Product entity)
    {
        List<ProductCategory> categories = new(entity.Categories.Count);
        
        var currectCategories = await _context
            .ProductCategories
            .AsNoTracking()
            .Where(pc => pc.ProductId == entity.Id)
            .ToListAsync();

        if (currectCategories.Any())
            _context.ProductCategories.RemoveRange(currectCategories);

        foreach (var category in entity.Categories)
        {
            var existingCategory = await _context.Categories.FindAsync(category.CategoryId);

            existingCategory ??= (await _context.Categories.AddAsync(category.Category)).Entity;

            categories.Add(new ProductCategory
            {
                CategoryId = existingCategory.Id,
                Category = existingCategory
            });
        }

        return categories;
    }

    public async Task<List<Product>> GetTopActiveAsync(int top)
    {
        return await GetProductsWithRelations()
            .Where(p => p.IsActive ?? true)
            .OrderByDescending(p => p.CreatedAt)
            .Take(top)
            .ToListAsync();
    }

    public async Task<List<Product>> GetByCategoryAndSearchAsync(string? search, string? category)
    {
        var query = GetProductsWithRelations();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Name != null && p.Name.ToLower().Contains(search.ToLower()));

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Categories.Any(c => c.CategoryId == category));

        return await query
            .OrderBy(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Product?> DisableAsync(Guid id) 
    {
        var item = await base.GetAsync(id);
        
        if (item == null)
            return null;

        item.IsActive = false;

        return await base.UpdateAsync(item);
    }

    public async Task<Product?> ActivateAsync(Guid id)
    {
        var item = await base.GetAsync(id);

        if (item == null)
            return null;

        item.IsActive = true;

        return await base.UpdateAsync(item);
    }

    public override Task<Product?> GetAsync(Guid id)
    {
        return GetProductsWithRelations()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<Product> UpdateAsync(Product entity)
    {
        // Find product variants that needs to be removed
        var current = await _context.Products
            .AsNoTracking()
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.Id == entity.Id);

        if (current == null)
            throw new Exception("Product not found");

        var removedVariants = current.Variants.Where(x => !entity.Variants.Any(y => y.Id == x.Id)).ToList();

        if (removedVariants.Count > 0)
            _context.ProductVariants.RemoveRange(removedVariants);

        // Get tracked categories
        entity.Categories = await GetTrackedCategoriesAsync(entity);

        return await base.UpdateAsync(entity);
    }

    public override async Task<Product> CreateAsync(Product entity)
    {
        // Get tracked categories
        entity.Categories = await GetTrackedCategoriesAsync(entity);

        return await base.CreateAsync(entity);
    }
        
    public Task<ProductVariant?> GetProductVariantByIdAsync(Guid variantId)
    {
        return _context
            .ProductVariants
            .FirstOrDefaultAsync(x => x.Id == variantId);
    }
}

public interface IProductRepository : IRepository<Guid, Product>
{
    Task<List<Product>> GetTopActiveAsync(int top);
    Task<List<Product>> GetByCategoryAndSearchAsync(string? search, string? category);
    Task<Product?> DisableAsync(Guid id);
    Task<Product?> ActivateAsync(Guid id);
    Task<ProductVariant?> GetProductVariantByIdAsync(Guid variantId);
}