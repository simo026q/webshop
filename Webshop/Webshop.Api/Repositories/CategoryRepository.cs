using Microsoft.EntityFrameworkCore;
using Webshop.Api.Contexts;
using Webshop.Api.Entities;
using Webshop.Api.Interfaces;

namespace Webshop.Api.Repositories;

public class CategoryRepository : RepositoryBase<string, Category>, ICategoryRepository
{
    private readonly WebshopContext _context;

    public CategoryRepository(WebshopContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllWithProductsAsync()
    {
        return await _context.Categories
            .Include(x => x.Products)
            .Where(x => x.Products.Count > 0)
            .ToListAsync();
    }

    public override async Task<Category?> DeleteAsync(string id)
    {
        var productCategories = _context.ProductCategories.Where(x => x.CategoryId == id);

        _context.ProductCategories.RemoveRange(productCategories);

        return await base.DeleteAsync(id);
    }
}

public interface ICategoryRepository : IRepository<string, Category>
{
    public Task<List<Category>> GetAllWithProductsAsync();
}