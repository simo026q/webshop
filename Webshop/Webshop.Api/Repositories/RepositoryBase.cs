using Microsoft.EntityFrameworkCore;
using Webshop.Api.Interfaces;

namespace Webshop.Api.Repositories;

public abstract class RepositoryBase<TKey, TEntity> : IRepository<TKey, TEntity>
    where TEntity : class, IUniqueEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly DbContext _context;

    public RepositoryBase(DbContext context)
    {
        _context = context;
    }
    
    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity?> GetAsync(TKey id)
    {
        return await _context.FindAsync<TEntity>(id);
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        var item = await _context.AddAsync(entity);

        await _context.SaveChangesAsync();
        
        return item.Entity;
    }
    
    public virtual async Task<TEntity?> DeleteAsync(TKey id)
    {
        var item = await GetAsync(id);
        
        if (item == null)
            return null;

        var removedItem = _context.Remove(item).Entity;

        await _context.SaveChangesAsync();

        return removedItem;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _context.Update(entity);

        await _context.SaveChangesAsync();

        return entity;
    }
}
