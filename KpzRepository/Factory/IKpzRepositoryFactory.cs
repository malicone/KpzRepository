using KpzRepository.Model;
using KpzRepository.Repository;

namespace KpzRepository.Factory;

public interface IKpzRepositoryFactory
{
    /// <summary>
    /// Creates base (minimum) repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>The created repository instance.</returns>
    IKpzRepository<TKey, TEntity> GetBaseRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>, new();
}