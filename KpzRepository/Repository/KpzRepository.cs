using Dapper;
using Dapper.Contrib.Extensions;
using KpzRepository.Model;
using System.Data;

namespace KpzRepository.Repository;

/// <inheritdoc cref="IKpzRepository{TKey, TEntity}"/>
/// <summary>
/// This class uses Dapper and Dapper.Contrib.Extensions libraries.
/// </summary>
public abstract class KpzRepository<TKey, TEntity> : IKpzRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>, new()
{
    public KpzRepository(IDbConnection connection)
    {
        Connection = connection;
    }
    public virtual IDbConnection? Connection { get; protected set; }
    public virtual bool OpenConnection()
    {
        if (Connection == null)
            return false;

        if (IsConnected == false)
        {
            try
            {
                Connection?.Open();
                return Connection?.State == ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }
        return true;
    }

    public virtual bool CloseConnection()
    {
        if (Connection == null)
            return false;
        if (Connection?.State == ConnectionState.Closed)
            return true;

        try
        {
            Connection?.Close();
            return Connection?.State == ConnectionState.Closed;
        }
        catch
        {
            return false;
        }
    }

    public virtual bool IsConnected { get => Connection?.State == ConnectionState.Open; }

    public virtual IDbTransaction? BeginTransaction()
    {
        if (OpenConnection())
        {
            return Connection?.BeginTransaction();
        }
        return null;
    }

    public virtual string GetRepositoryTableName()
    {
        var entity = new TEntity();
        return entity.GetTableName();
    }


    public virtual string GetRepositoryKeyName()
    {
        var entity = new TEntity();
        return entity.GetKeyName();
    }

    public virtual bool Add(TEntity entity, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            Connection.Insert(entity, transaction);
            return true;
        }
        return false;
    }

    public virtual async Task<bool> AddAsync(TEntity entity, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            await Connection.InsertAsync(entity, transaction);
            return true;
        }
        return false;
    }

    public virtual long AddRange(IEnumerable<TEntity> entities, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            return Connection.Insert(entities, transaction);
        }
        return 0;
    }

    public virtual async Task<long> AddRangeAsync(IEnumerable<TEntity> entities, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            return await Connection.InsertAsync(entities, transaction);
        }
        return 0;
    }

    public virtual long Count(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            string sql = $"SELECT COUNT(*) FROM {GetRepositoryTableName()}";
            var result = Connection?.ExecuteScalar<long>(sql, null, transaction);
            if (result != null)
            {
                return result.Value;
            }
        }
        return 0;
    }

    public virtual async Task<long> CountAsync(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            string sql = $"SELECT COUNT(*) FROM {GetRepositoryTableName()}";
            return await Connection!.ExecuteScalarAsync<long>(sql, null, transaction);
        }
        return 0;
    }

    public virtual bool Delete(TKey id, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            TEntity entityToDelete = Connection.Get<TEntity>(id, transaction);
            if (entityToDelete == null)
                return false;
            return Connection.Delete(entityToDelete, transaction);
        }
        return false;
    }

    public virtual bool DeleteAll(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            return Connection.DeleteAll<TEntity>(transaction);
        }
        return false;
    }

    public virtual bool IsEmpty(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            return Count(transaction) == 0;
        }
        return true;
    }

    public virtual long ExecuteQuery(string sql, object? param = null, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            var result = Connection?.Execute(sql, param, transaction);
            if (result != null)
            {
                return result.Value;
            }
        }
        return 0;
    }
    public virtual async Task<long> ExecuteQueryAsync(string sql, object? param = null, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            return await Connection?.ExecuteAsync(sql, param, transaction)!;
        }
        return 0;
    }

    public virtual bool Exists(TKey id, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            return Connection.Get<TEntity>(id, transaction) != null;
        }
        return false;
    }
    public virtual async Task<bool> ExistsAsync(TKey id, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            return await Connection?.GetAsync<TEntity>(id, transaction)! != null;
        }
        return false;
    }

    public virtual TEntity? Get(TKey id, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            return Connection?.Get<TEntity>(id, transaction);
        }
        return null;
    }

    public virtual async Task<TEntity?> GetAsync(TKey id, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            return await Connection?.GetAsync<TEntity>(id, transaction)!;
        }
        return null;
    }

    /// <summary>
    /// Selects all entities from the db.
    /// </summary>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>All entities as Enumerable or empty enumerable if nothing found.</returns>
    public virtual IEnumerable<TEntity> GetAll(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            var selectedEntities = Connection.GetAll<TEntity>(transaction);
            return selectedEntities ?? Enumerable.Empty<TEntity>();
        }
        return Enumerable.Empty<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            var selectedEntities = await Connection.GetAllAsync<TEntity>(transaction);
            return selectedEntities ?? Enumerable.Empty<TEntity>();
        }
        return Enumerable.Empty<TEntity>();
    }

    public virtual IEnumerable<TEntity> GetAllOrderBy(string? columnName = null, bool desc = false, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            string sql = ComposeOrderByQuery(columnName, desc);
            var foundEntities = Connection?.Query<TEntity>(sql, null, transaction);
            return foundEntities ?? Enumerable.Empty<TEntity>();
        }
        return Enumerable.Empty<TEntity>();
    }

    protected virtual string ComposeOrderByQuery(string? sortColumnName = null, bool desc = false)
    {
        var entity = new TEntity();
        string tableName = GetRepositoryTableName();
        string actualSortColumnName = sortColumnName ?? entity.GetDefaultSortFieldName();
        string orderClause = entity.IsFieldTypeOfString(actualSortColumnName)
            ? $"UPPER({actualSortColumnName})" : $"{actualSortColumnName}";
        if (desc)
        {
            return $"SELECT * FROM {tableName} ORDER BY {orderClause} DESC";
        }
        return $"SELECT * FROM {tableName} ORDER BY {orderClause} ASC";
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllOrderByAsync(string? columnName = null,
        bool desc = false, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            string sql = ComposeOrderByQuery(columnName, desc);
            var foundEntities = await Connection?.QueryAsync<TEntity>(sql, null, transaction)!;
            return foundEntities ?? Enumerable.Empty<TEntity>();
        }
        return Enumerable.Empty<TEntity>();
    }

    public virtual TEntity? GetMinEntity(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            TKey? minId = GetMinId(transaction);
            if (minId != null)
                return Get(minId, transaction);
        }
        return null;
    }
    public virtual TEntity? GetMaxEntity(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            TKey? maxId = GetMaxId(transaction);
            if (maxId != null)
                return Get(maxId, transaction);
        }
        return null;
    }

    public virtual IEnumerable<TEntity> GetEntitiesLike(string fieldName, string searchText,
        bool descOrder = false, bool groupBy = false, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            string sql = ComposeLikeQuery(fieldName, searchText, descOrder, groupBy);
            var foundEntities = Connection?.Query<TEntity>(sql, null, transaction);
            return foundEntities ?? Enumerable.Empty<TEntity>();
        }
        return Enumerable.Empty<TEntity>();
    }

    protected virtual string ComposeLikeQuery(string fieldName, string searchText,
        bool descOrder = false, bool groupBy = false)
    {
        string groupByClause = groupBy ? $"GROUP BY {fieldName}" : string.Empty;
        string orderDirection = descOrder ? "DESC" : "ASC";
        return $@"SELECT TRIM({fieldName}) FROM {GetRepositoryTableName()} WHERE UPPER(TRIM({fieldName})) 
                LIKE '%{searchText.ToUpper()}%' {groupByClause} ORDER BY {fieldName} {orderDirection}";
    }

    public virtual async Task<IEnumerable<TEntity>> GetEntitiesLikeAsync(string fieldName, string searchText,
        bool descOrder = false, bool groupBy = false, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            string sql = ComposeLikeQuery(fieldName, searchText, descOrder, groupBy);
            var foundEntities = await Connection?.QueryAsync<TEntity>(sql, null, transaction)!;
            return foundEntities ?? Enumerable.Empty<TEntity>();
        }
        return Enumerable.Empty<TEntity>();
    }

    /// <summary>
    /// Returns max id from the table.
    /// </summary>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if 
    /// no transactions are used.</param>
    /// <returns>Max id or default key type value.</returns>
    public virtual TKey? GetMaxId(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            string sql = $"SELECT MAX({GetRepositoryKeyName()}) FROM {GetRepositoryTableName()}";
            var result = Connection?.ExecuteScalar(sql, null, transaction);
            if ((result != null) && (result is TKey key))
            {
                return key;
            }
        }
        return default;
    }

    public virtual TKey? GetMinId(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            string sql = $"SELECT MIN({GetRepositoryKeyName()}) FROM {GetRepositoryTableName()}";
            var result = Connection?.ExecuteScalar(sql, null, transaction);
            if ((result != null) && (result is TKey key))
            {
                return key;
            }
        }
        return default;
    }

    public virtual bool Update(TEntity entity, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            return Connection.Update(entity, transaction);
        }
        return false;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            return await Connection.UpdateAsync(entity, transaction);
        }
        return false;
    }

    public virtual TKey? GetLastInsertedId(IDbTransaction? transaction = null)
    {
        throw new NotImplementedException("Not implemented in base repository. Please override in derived repository or use a " +
            "specific implementation like KpzRepository.SqlServer.");
    }
}