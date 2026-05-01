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

    public override TKey GetLastInsertedId(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            // SCOPE_IDENTITY() - Returns the last identity for the current session and current scope. Recommended for most uses.
            // @@IDENTITY - Returns the last identity for the current session across any scope. If a trigger inserts a row, this returns the trigger's ID, not yours.
            // IDENT_CURRENT('table') - Returns the last identity for a specific table across any session. Risky because another user's insert could change
            // this value before you read it.
            string sql = $"SELECT IDENT_CURRENT('{GetRepositoryTableName()}') AS LastInsertedId";
            var result = Connection!.ExecuteScalar<TKey>(sql, null, transaction);
            if (result != null)
            {
                return result;
            }
        }
        return default!;
    }
}