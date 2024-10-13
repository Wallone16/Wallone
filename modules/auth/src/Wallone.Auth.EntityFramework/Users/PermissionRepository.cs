using Microsoft.EntityFrameworkCore;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.EntityFramework.EntityFramework;

namespace Wallone.Auth.EntityFramework.Users
{
    internal sealed class PermissionRepository : IPermissionRepository
    {
        private readonly AuthDbContext _context;

        public PermissionRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<HashSet<string>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var roles = await _context.Users
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Permissions)
                .Where(x => x.Id == userId)
                .Select(x => x.Roles)
                .ToArrayAsync(cancellationToken);

            return roles
                .SelectMany(x => x)
                .SelectMany(x => x.Permissions)
                .Select(x => x.Name)
                .ToHashSet();
        }
    }
}