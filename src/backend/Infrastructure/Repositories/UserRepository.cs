using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class UserRepository(DbContext dbContext) : RepositoryBase<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) => 
        await _dbSet.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
        => !await _dbSet.AnyAsync(x => x.Email == email, cancellationToken);

    public async Task<IEnumerable<string>> GetPermissions(Guid id, CancellationToken cancellationToken)
    { 
        return await _dbSet
            .Include(user => user.Roles) 
            .ThenInclude(role => role.Permissions) 
            .Where(user => user.Id == id) 
            .SelectMany(user => user.Roles) 
            .SelectMany(role => role.Permissions) 
            .Select(permission => permission.Name) 
            .Distinct() 
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }
}