using KpzRepository.Factory;
using KpzRepository.Model;
using KpzRepository.Repository;
using KpzRepository.Sqlite.Repository;
using Microsoft.Data.Sqlite;

namespace KpzRepository.Sqlite.Factory;

/// <summary>
/// <inheritdoc cref="IKpzRepositoryFactory{TKey, TEntity}"/>
/// Factory class for creating SQLite repositories.
/// </summary>
public class KpzRepositorySqliteFactory : IKpzRepositoryFactory
{
    public KpzRepositorySqliteFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public IKpzRepository<TKey, TEntity> GetBaseRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>, new()
    {
        return new KpzRepositorySqlite<TKey, TEntity>(GetNewConnection(ConnectionString));
    }

    protected virtual SqliteConnection GetNewConnection(string connectionString)
    {
        var newConnection = new SqliteConnection(connectionString);
        return newConnection;
    }

    protected virtual string ConnectionString { get; set; } = string.Empty;
}