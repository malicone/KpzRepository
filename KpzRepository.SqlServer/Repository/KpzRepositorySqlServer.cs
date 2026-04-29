using Dapper;
using KpzRepository.Model;
using KpzRepository.Repository;
using System.Data;

namespace KpzRepository.SqlServer.Repository;

public class KpzRepositorySqlServer<TKey, TEntity> : KpzRepository<TKey, TEntity>, IKpzRepository<TKey, TEntity>
    where TEntity : BaseEntity<TKey>, new()
{
    public KpzRepositorySqlServer(IDbConnection connection) : base(connection)
    {

    }

    public override TKey? GetLastInsertedId(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            string sql = $"SELECT SCOPE_IDENTITY() AS LastInsertedId";
            var result = Connection!.ExecuteScalar<TKey?>(sql, null, transaction);
            if (result != null)
            {
                return (TKey?)result;
            }
        }
        return default;
    }
}