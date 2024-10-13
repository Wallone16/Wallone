using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.EntityFramework.EntityFramework;

namespace Wallone.Auth.EntityFramework.Users.Configurations
{
    internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermissions", EntityFrameworkSettings.DbScheme);

            builder.HasKey(x => new { x.RoleId, x.PermissionId });

            builder.HasData(SeedRolePermissions());
        }

        private IEnumerable<RolePermission> SeedRolePermissions()
        {
            var rolePermissions = new List<RolePermission>
            {
                new(Role.Admin.Id, (int)Shared.Domain.Permission.ReadWallpaper),
                new(Role.Admin.Id, (int)Shared.Domain.Permission.DeleteWallpaper),
                new(Role.Admin.Id, (int)Shared.Domain.Permission.CreateWallpaper)
            };

            return rolePermissions;
        }
    }
}