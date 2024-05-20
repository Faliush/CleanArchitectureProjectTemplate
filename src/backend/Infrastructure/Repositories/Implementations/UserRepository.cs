using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementations;

internal sealed class UserRepository(ApplicationDbContext dbContext) : RepositoryBase<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken) =>
        await GetFirstOrDefaultAsync(
            predicate: x => x.Email.Value == email,
            disableTracking: true,
            cancellationToken: cancellationToken);

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken) =>
        !await _dbSet.AnyAsync(x => x.Email.Value == email, cancellationToken);
}
