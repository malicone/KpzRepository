using KpzRepository.Model;
using System.Data;

namespace KpzRepository.Repository;

/// <summary>
/// Base repository interface. All basic CRUD and select operations are listed here.
/// So by default we get all basic operations by simple initializing. For example:
/// IKpzRepository<long, Product> repository = new KpzRepository<long, Product>().
/// But better use IKpzRepositoryFactory to create repository instances.
/// </summary>
/// <typeparam name="TKey">Type of primary key.</typeparam>
/// <typeparam name="TEntity">Entity is mapped on table in the db.</typeparam>
public interface IKpzRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>, new()
{
    /// <summary>
    /// The db connection which is used to perform all operations.
    /// </summary>
    IDbConnection? Connection { get; }

    /// <summary>
    /// Opens the db connection.
    /// </summary>
    /// <returns>true if connection successfully opened otherwise false.</returns>
    bool OpenConnection();

    /// <summary>
    /// Closes the db connection.
    /// </summary>
    /// <returns>true if connection successfully closed otherwise false.</returns>
    bool CloseConnection();

    /// <summary>
    /// Indicates whether the db connection is currently open.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Starts a new transaction. It's better to run multiple operations within a transaction to avoid primary key violation and to increase performance.
    /// </summary>
    /// <returns>The started transaction.</returns>
    IDbTransaction? BeginTransaction();

    /// <summary>
    /// Gets the name of the repository table (table in db on which the repository is mapped).
    /// By default, the repository table name is the same as the entity's (class) name (like Product, User etc.).
    /// But you can override it if you want to use different table name for repository (using [Table] attribute).
    /// </summary>
    /// <returns>The repository table name.</returns>
    string GetRepositoryTableName();

    /// <summary>
    /// Gets the repository primary key name.
    /// </summary>
    /// <returns>The repository primary key name.</returns>
    string GetRepositoryKeyName();

    /// <summary>
    /// Inserts entity into db in the mapped table.
    /// </summary>
    /// <param name="entity">Entity to be inserted.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>true if entity successfully inserted otherwise false.</returns>
    bool Add(TEntity entity, IDbTransaction? transaction = null);

    /// <summary>
    /// Inserts entity into db in the mapped table in async mode.
    /// </summary>
    /// <param name="entity">Entity to be inserted.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>true if entity successfully inserted otherwise false.</returns>    
    Task<bool> AddAsync(TEntity entity, IDbTransaction? transaction = null);

    /// <summary>
    /// Inserts list of entities into the db in the mapped table. 
    /// Important! It's better to run this method within a transaction to avoid primary key violation.
    /// </summary>
    /// <param name="entities">Entities to be inserted.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>Number of inserted entities.</returns>
    long AddRange(IEnumerable<TEntity> entities, IDbTransaction? transaction = null);

    /// <summary>
    /// Inserts list of entities into the db in the mapped in async mode.
    /// Important! It's better to run this method within a transaction to avoid primary key violation.
    /// </summary>
    /// <param name="entities">Entities to be inserted.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>Number of inserted entities.</returns>
    Task<long> AddRangeAsync(IEnumerable<TEntity> entities, IDbTransaction? transaction = null);

    /// <summary>
    /// Updates entity in the db in the mapped table. Entity is searched by id and then updated with new values.
    /// </summary>
    /// <param name="entity">Entity to be updated.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>true if entity successfully updated otherwise false.</returns>
    bool Update(TEntity entity, IDbTransaction? transaction = null);

    /// <summary>
    /// Updates entity in the db in the mapped table in async mode. Entity is searched by id and then updated with new values.
    /// </summary>
    /// <param name="entity">Entity to be updated.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>true if entity successfully updated otherwise false.</returns>
    Task<bool> UpdateAsync(TEntity entity, IDbTransaction? transaction = null);

    bool Delete(TKey id, IDbTransaction? transaction = null);
    bool DeleteAll(IDbTransaction? transaction = null);
    bool IsEmpty(IDbTransaction? transaction = null);

    /// <summary>
    /// Searches entity in the db in the mapped table by id.
    /// </summary>
    /// <param name="id">Id to search by.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>Found entity or null if nothing found.</returns>
    TEntity? Get(TKey id, IDbTransaction? transaction = null);

    /// <summary>
    /// Searches entity in the db in the mapped table by id in async mode.
    /// </summary>
    /// <param name="id">Id to search by.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>Found entity or null if nothing found.</returns>
    Task<TEntity?> GetAsync(TKey id, IDbTransaction? transaction = null);

    /// <summary>
    /// Retrieves all entities from the db in the mapped table.
    /// </summary>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>All entities from the db in the mapped table.</returns>
    IEnumerable<TEntity> GetAll(IDbTransaction? transaction = null);

    /// <summary>
    /// Retrieves all entities from the db in the mapped table in async mode.
    /// </summary>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>All entities from the db in the mapped table.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(IDbTransaction? transaction = null);

    IEnumerable<TEntity> GetAllOrderBy(string? columnName = null, bool desc = false, IDbTransaction? transaction = null);
    Task<IEnumerable<TEntity>> GetAllOrderByAsync(string? columnName = null, bool desc = false, IDbTransaction? transaction = null);
    TEntity? GetMinEntity(IDbTransaction? transaction = null);
    TEntity? GetMaxEntity(IDbTransaction? transaction = null);
    IEnumerable<TEntity> GetEntitiesLike(string fieldName, string searchText, bool desc = false,
        bool groupBy = false, IDbTransaction? transaction = null);
    Task<IEnumerable<TEntity>> GetEntitiesLikeAsync(string fieldName, string searchText, bool desc = false,
        bool groupBy = false, IDbTransaction? transaction = null);

    /// <summary>
    /// Counts the number of entities in the db in the mapped table.
    /// </summary>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>The number of entities in the db in the mapped table.</returns>
    long Count(IDbTransaction? transaction = null);
    
    /// <summary>
    /// Counts the number of entities in the db in the mapped table in async mode.
    /// </summary>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>The number of entities in the db in the mapped table.</returns>
    Task<long> CountAsync(IDbTransaction? transaction = null);

    /// <summary>
    /// Returns last inserted id in the db in the mapped table. It can be useful when you need to insert entity and then get its id to do some operations with it.
    /// </summary>
    /// <remarks>This method is database specific and may not be supported by all databases. The method throws exception in KpzRepository implementation.</remarks>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>Last inserted id or null if no entities exist.</returns>
    TKey? GetLastInsertedId(IDbTransaction? transaction = null);

    /// <summary>
    /// Returns max id from the mapped table.
    /// </summary>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if 
    /// no transactions are used.</param>
    /// <returns>Max id or null if no entities exist.</returns>
    TKey? GetMaxId(IDbTransaction? transaction = null);

    /// <summary>
    /// Returns min id from the mapped table.
    /// </summary>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if 
    /// no transactions are used.</param>
    /// <returns>Min id or null if no entities exist.</returns>
    TKey? GetMinId(IDbTransaction? transaction = null);

    /// <summary>
    /// Checks if entity exists in the db in the mapped table.
    /// </summary>
    /// <param name="id">Id of the entity to check.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>true if entity exists otherwise false.</returns>
    bool Exists(TKey id, IDbTransaction? transaction = null);

    /// <summary>
    /// Checks if entity exists in the db in the mapped table in async mode.
    /// </summary>
    /// <param name="id">Id of the entity to check.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>true if entity exists otherwise false.</returns>
    Task<bool> ExistsAsync(TKey id, IDbTransaction? transaction = null);

    /// <summary>
    /// Executes sql query.
    /// </summary>
    /// <param name="sql">The query text.</param>
    /// <param name="param">Parameters to pass to query.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>The number of rows affected.</returns>
    long ExecuteQuery(string sql, object? param = null, IDbTransaction? transaction = null);

    /// <summary>
    /// Executes sql query in async mode.
    /// </summary>
    /// <param name="sql">The query text.</param>
    /// <param name="param">Parameters to pass to query.</param>
    /// <param name="transaction">Started transaction the method to be run in. You can ignore it if no transactions are used.</param>
    /// <returns>The number of rows affected.</returns>
    Task<long> ExecuteQueryAsync(string sql, object? param = null, IDbTransaction? transaction = null);
}