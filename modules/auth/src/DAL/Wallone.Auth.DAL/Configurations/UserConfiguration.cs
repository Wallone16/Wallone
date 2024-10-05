using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallone.Identity.Domain.User;

namespace Wallone.Auth.DAL.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(x => x.UserName)
                .HasMaxLength(50)
                .IsRequired();
            builder
                .Property(x => x.Email)
                .HasMaxLength(200)
                .IsRequired();
            builder
                .Property(x => x.Password)
                .HasMaxLength(64)
                .IsRequired();

            builder
                .HasOne(x => x.UserRole)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}