namespace Application.Abstractions.Authentication.PermissionService;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsAsync(Guid userId);
}
