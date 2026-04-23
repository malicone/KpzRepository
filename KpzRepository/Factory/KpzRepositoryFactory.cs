using KpzRepository.Model;
using KpzRepository.Repository;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KpzRepository.Factory;

/// <summary>
/// <inheritdoc cref="IKpzRepositoryFactory"/>
/// </summary>
public class KpzRepositoryFactory : IKpzRepositoryFactory
{
    public KpzRepositoryFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public IKpzRepository<TKey, TEntity> GetBaseRepository<TKey, TEntity>() 
        where TEntity : BaseEntity<TKey>, new()
    {
        return new KpzRepository<TKey, TEntity>(GetNewConnection(ConnectionString));        
    }

    protected virtual SqlConnection GetNewConnection(string connectionString)
    {
        var newConnection = new SqlConnection(connectionString);
        return newConnection;
    }

    protected virtual string ConnectionString { get; set; } = string.Empty;    
}