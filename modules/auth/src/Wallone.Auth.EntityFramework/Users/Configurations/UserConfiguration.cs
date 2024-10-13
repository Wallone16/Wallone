using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallone.Auth.Domain.Shared.Users.Constants;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.EntityFramework.EntityFramework;

namespace Wallone.Auth.EntityFramework.Users.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", EntityFrameworkSettings.DbScheme);

            builder
                .Property(x => x.UserName)
                .HasMaxLength(UserConstants.UserNameMaxLength)
                .IsRequired();
            builder
                .Property(x => x.Email)
                .HasMaxLength(UserConstants.EmailMaxLength)
                .IsRequired();
            builder
                .Property(x => x.Password)
                .HasMaxLength(UserConstants.PasswordMaxLength)
                .IsRequired();
        }
    }
}