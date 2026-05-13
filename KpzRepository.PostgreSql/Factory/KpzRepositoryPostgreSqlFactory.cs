using KpzRepository.Factory;
using KpzRepository.Model;
using KpzRepository.PostgreSql.Repository;
using KpzRepository.Repository;
using Npgsql;

namespace KpzRepository.PostgreSql.Factory;

/// <summary>
/// <inheritdoc cref="IKpzRepositoryFactory{TKey, TEntity}"/>
/// Factory class for creating PostgreSQL repositories.
/// </summary>
internal class KpzRepositoryPostgreSqlFactory : IKpzRepositoryFactory
{
    public KpzRepositoryPostgreSqlFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public IKpzRepository<TKey, TEntity> GetBaseRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>, new()
    {
        return new KpzRepositoryPostgreSql<TKey, TEntity>(GetNewConnection(ConnectionString));
    }

    protected virtual NpgsqlConnection GetNewConnection(string connectionString)
    {
        var newConnection = new NpgsqlConnection(connectionString);
        return newConnection;
    }

    protected virtual string ConnectionString { get; set; } = string.Empty;
}