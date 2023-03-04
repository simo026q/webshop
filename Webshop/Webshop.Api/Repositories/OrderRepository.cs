using Webshop.Api.Extensions;
using Webshop.Api.Interfaces;
using Webshop.Api.Entities;
using Webshop.Api.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Webshop.Api.Repositories;

public class OrderRepository : RepositoryBase<Guid, Order>, IOrderRepository
{
    private readonly WebshopContext _context;
    
    public OrderRepository(WebshopContext context) : base(context)
    {
        _context = context;
    }

    private IQueryable<Order> GetProductsWithRelations()
    {
        return _context
            .Orders
            .Include(o => o.User)
            .Include(o => o.Address)
            .Include(o => o.Products)
            .ThenInclude(o => o.ProductVariant);
    }

    public Task<List<Order>> GetAllByUserIdAsync(Guid userId)
    {
        return GetProductsWithRelations()
            .Where(o => o.UserId == userId)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();
    }

    public override Task<List<Order>> GetAllAsync()
    {
        return GetProductsWithRelations()
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();
    }

    public override Task<Order?> GetAsync(Guid id)
    {
        return GetProductsWithRelations()
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}

public interface IOrderRepository : IRepository<Guid, Order>
{
    Task<List<Order>> GetAllByUserIdAsync(Guid userId);
}