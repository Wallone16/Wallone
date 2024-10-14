using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.EntityFramework.EntityFramework;

namespace Wallone.Auth.EntityFramework.Users.Configurations
{
    internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions", EntityFrameworkSettings.DbScheme);

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder.HasData(SeedPermissions());
        }

        private IEnumerable<Permission> SeedPermissions()
        {
            var permissions = Enum.GetValues<Shared.Domain.Permission>()
                .Select(x => new Permission(
                    id: (int)x,
                    name: x.ToString())
                );

            return permissions;
        }
    }
}
