
namespace Wallone.Shared.EntityFramework.Repositories
{
    internal sealed class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    {
        public Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}