using Domain.Entities;

namespace Application.Abstractions.Authentication.PermissionService;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsAsync(User user);
}
