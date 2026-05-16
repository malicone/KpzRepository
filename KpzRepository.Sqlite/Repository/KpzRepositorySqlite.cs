using KpzRepository.Model;
using KpzRepository.Repository;
using System.Data;
using Dapper;

namespace KpzRepository.Sqlite.Repository;

/// <summary>
/// SQLite implementation of the repository. It inherits from the base repository and implements specific methods of SQLite.
/// <inheritdoc cref="KpzRepository{TKey, TEntity}"/>
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
/// <typeparam name="TEntity">The type of the entity (entity is mapped to a database table).</typeparam>
public class KpzRepositorySqlite<TKey, TEntity> : KpzRepository<TKey, TEntity>, IKpzRepository<TKey, TEntity>
    where TEntity : BaseEntity<TKey>, new ()
{
    public KpzRepositorySqlite(IDbConnection connection) : base(connection)
    {

    }
    public override TKey GetLastInsertedId(IDbTransaction? transaction = null)
    {
        if(OpenConnection())
        {
            string sql = $"SELECT last_insert_rowid() AS LastInsertedId";
            var result = Connection!.ExecuteScalar<TKey>(sql, null, transaction);
            if(result != null)
            {
                return result;
            }
        }
        return default!;
    }
}