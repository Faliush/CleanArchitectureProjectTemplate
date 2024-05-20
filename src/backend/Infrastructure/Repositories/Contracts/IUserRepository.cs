using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories.Contracts.Base;


namespace Infrastructure.Repositories.Contracts;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken);

    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken);
}
