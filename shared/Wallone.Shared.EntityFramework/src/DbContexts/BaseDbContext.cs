using Microsoft.EntityFrameworkCore;

namespace Wallone.Shared.EntityFramework.src.DbContexts
{
    public abstract class BaseDbContext : DbContext
    {
        protected BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<T> GetDbSet<T>() where T : BaseEntity
        {
            return Set<T>();
        }
    }
}