namespace Wallone.Auth.Services.Contracts.Users
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}