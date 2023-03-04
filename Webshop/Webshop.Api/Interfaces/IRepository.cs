using Microsoft.EntityFrameworkCore;

namespace Webshop.Api.Interfaces;

public interface IRepository<TKey, TEntity>
    where TEntity : class, IUniqueEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">The <typeparamref name="TKey"/> id of the <typeparamref name="TEntity"/></param>
    /// <returns>Returns the <typeparamref name="TEntity"/> with that <paramref name="id"/></returns>
    Task<TEntity?> GetAsync(TKey id);

    /// <summary>
    /// Get all entities of <typeparamref name="TEntity"/> from the context.
    /// </summary>
    /// <returns>Returns a list of <typeparamref name="TEntity"/> in the <see cref="DbSet{TEntity}"/></returns>
    Task<List<TEntity>> GetAllAsync();

    /// <summary>
    /// Add an <typeparamref name="TEntity"/> to the context.
    /// </summary>
    /// <param name="entity">A <typeparamref name="TEntity"/>.</param>
    /// <returns>Returns the <typeparamref name="TEntity"/> created.</returns>
    /// <exception cref="DbUpdateException"/>
    /// <exception cref="DbUpdateConcurrencyException"/>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// Update a <typeparamref name="TEntity"/> in the context.
    /// </summary>
    /// <param name="entity">A <typeparamref name="TEntity"/>.</param>
    /// <returns>Return the updated <typeparamref name="TEntity"/>.</returns>
    /// <exception cref="DbUpdateException"/>
    /// <exception cref="DbUpdateConcurrencyException"/>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Delete a <typeparamref name="TEntity"/> from the context.
    /// </summary>
    /// <param name="id">The <typeparamref name="TKey"/> id of the <typeparamref name="TEntity"/></param>
    /// <returns>Returns the <typeparamref name="TEntity"/> deleted.</returns>
    /// <exception cref="DbUpdateException"/>
    /// <exception cref="DbUpdateConcurrencyException"/>
    Task<TEntity?> DeleteAsync(TKey id);
}
