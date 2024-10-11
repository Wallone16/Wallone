namespace Wallone.Shared.EntityFramework.Repositories
{
    public interface IBaseRepository<TEntity, TKey>
    {
        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}