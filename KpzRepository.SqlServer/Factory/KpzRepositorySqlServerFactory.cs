using KpzRepository.Factory;
using KpzRepository.Model;
using KpzRepository.Repository;
using KpzRepository.SqlServer.Repository;
using Microsoft.Data.SqlClient;

namespace KpzRepository.SqlServer.Factory;

/// <summary>
/// <inheritdoc cref="IKpzRepositoryFactory{TKey, TEntity}"/>
/// Factory class for creating SQL Server repositories.
/// </summary>
internal class KpzRepositorySqlServerFactory : IKpzRepositoryFactory
{
    public KpzRepositorySqlServerFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public IKpzRepository<TKey, TEntity> GetBaseRepository<TKey, TEntity>()
        where TEntity : BaseEntity<TKey>, new()
    {
        return new KpzRepositorySqlServer<TKey, TEntity>(GetNewConnection(ConnectionString));
    }

    protected virtual SqlConnection GetNewConnection(string connectionString)
    {
        var newConnection = new SqlConnection(connectionString);
        return newConnection;
    }

    protected virtual string ConnectionString { get; set; } = string.Empty;
}
