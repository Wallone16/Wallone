using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Wallone.Auth.Domain.Users;

namespace Wallone.Auth.EntityFramework.EntityFramework
{
    internal sealed class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; private set; }
        public DbSet<UserRole> UserRoles { get; private set; }
        public DbSet<Role> Roles { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}