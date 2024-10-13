using Wallone.Auth.Domain.Users;
using Wallone.Auth.Services.Contracts.Users;

namespace Wallone.Auth.Services.Users
{
    internal sealed class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public Task<HashSet<string>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return _permissionRepository
                .GetPermissionsAsync(userId, cancellationToken);
        }
    }
}