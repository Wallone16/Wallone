using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallone.Auth.Domain.Users;

namespace Wallone.Auth.DAL.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder
                .Property(x => x.InternalName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .HasOne(x => x.UserRole)
                .WithOne(x => x.Role)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}