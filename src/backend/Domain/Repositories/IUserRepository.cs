using Domain.Entities;
using Domain.Repositories.Base;

namespace Domain.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetPermissions(Guid id, CancellationToken cancellationToken);
}