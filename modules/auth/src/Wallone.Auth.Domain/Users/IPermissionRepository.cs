namespace Wallone.Auth.Domain.Users
{
    public interface IPermissionRepository
    {
        Task<HashSet<string>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}