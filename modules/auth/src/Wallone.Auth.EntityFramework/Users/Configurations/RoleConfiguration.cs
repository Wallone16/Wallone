using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallone.Auth.Domain.Shared.Users.Constants;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.EntityFramework.EntityFramework;

namespace Wallone.Auth.EntityFramework.Users.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles", EntityFrameworkSettings.DbScheme);

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .HasMaxLength(RoleConstants.NameMaxLength)
                .IsRequired();
            builder
                .Property(x => x.InternalName)
                .HasMaxLength(RoleConstants.InternalNameMaxLength)
                .IsRequired();

            builder
                .HasMany(x => x.Permissions)
                .WithMany()
                .UsingEntity<RolePermission>();

            builder
                .HasMany(x => x.Users)
                .WithMany(x => x.Roles)
                .UsingEntity<UserRole>();

            builder.HasData(SeedRoles());
        }

        private IEnumerable<Role> SeedRoles()
        {
            var roles = new List<Role>
            {
                Role.Admin,
                Role.User
            };

            return roles;
        }
    }
}