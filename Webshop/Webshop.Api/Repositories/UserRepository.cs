using Microsoft.EntityFrameworkCore;
using Webshop.Api.Contexts;
using Webshop.Api.Entities;
using Webshop.Api.Interfaces;

namespace Webshop.Api.Repositories;

public class UserRepository : RepositoryBase<Guid, User>, IUserRepository
{
    private readonly WebshopContext _context;

    public UserRepository(WebshopContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public override async Task<List<User>> GetAllAsync()
    {
        return await _context
            .Users
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();
    }
}

public interface IUserRepository : IRepository<Guid, User>
{
    Task<User?> GetByEmailAsync(string email);
}
