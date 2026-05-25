# KpzRepository.PostgreSql

![KpzRepository](../images/KpzRepository_lib_icon.png)

A lightweight and flexible repository pattern implementation for .NET 8, providing a unified interface for database operations with PostgreSQL. Built on top of Dapper and Dapper.Contrib, KpzRepository simplifies data access while maintaining performance and flexibility.

## Table of Contents

- [Installation](#installation)
- [Quick Start](#quick-start)
- [Best Practices](#best-practices)
- [Entity Attributes](#entity-attributes)
- [Repository Interface Overview](#repository-interface-overview)
- [Extending Repository with Custom Methods](#extending-repository-with-custom-methods)
- [Implementing Custom Database Provider](#implementing-custom-database-provider)
- [Contributing](#contributing)
- [License](#license)
- [Links](#links)
- [Author](#author)

## Installation

Install only the database provider you need - the core package **KpzRepository** is included automatically:

```bash
dotnet add package KpzRepository.PostgreSql
```

## Quick Start

### ⚠️ Critical: snake_case is Required for PostgreSQL

**PostgreSQL converts unquoted identifiers to lowercase**, which means PascalCase properties will cause mapping errors with Dapper.Contrib. You **must** use snake_case for all property names and table names, or you will get exceptions like:

```
Column 'Name' does not exist
```

### 1. Define Your Entity

Create a class that inherits from `BaseEntity<TKey>` with **snake_case properties**:

```csharp
using Dapper.Contrib.Extensions;
using KpzRepository.Model;

[Table("products")]  // snake_case table name
public class Product : BaseEntity<long>
{
    [Key]  // You need to specify [Key] for auto-incrementing primary keys (SERIAL, BIGSERIAL)
    public long id { get; set; }  // snake_case property names

    public string name { get; set; } = null!;
    public string? description { get; set; }
    public decimal price { get; set; }
    public int quantity { get; set; }
    public bool is_active { get; set; }
    public DateTime created_at { get; set; }

    // PostgreSQL-specific: JSONB support
    public string? metadata { get; set; }  // Store JSON data as JSONB
}
```

**Corresponding PostgreSQL Table:**

```sql
CREATE TABLE products (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    price NUMERIC(10, 2) NOT NULL,
    quantity INTEGER NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    metadata JSONB
);
```

### 2. Create Repository Instance

#### Direct Creation

```csharp
using KpzRepository.Factory;
using KpzRepository.Repository;
using KpzRepository.PostgreSql.Factory;

// Create factory
string connectionString = "Host=localhost;Database=mydb;Username=postgres;Password=postgres;";
IKpzRepositoryFactory factory = new KpzRepositoryPostgreSqlFactory(connectionString);

// Get repository for your entity
IKpzRepository<long, Product> repository = factory.GetBaseRepository<long, Product>();

// Add a product
var product = new Product
{
    name = "Laptop",
    description = "High-performance laptop",
    price = 999.99m,
    quantity = 50,
    is_active = true,
    created_at = DateTime.UtcNow,
    metadata = "{\"brand\": \"Dell\", \"warranty\": \"2 years\"}"
};

repository.Add(product);
Console.WriteLine($"Product added with ID: {product.id}");

// Get all products
var products = repository.GetAll();
foreach (var p in products)
{
    Console.WriteLine($"{p.name} - ${p.price}");
}

// Update product
product.price = 899.99m;
repository.Update(product);

// Delete product
repository.Delete(product.id);

// Cleanup
repository.Dispose();
```

#### Using Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using KpzRepository;
using KpzRepository.Factory;
using KpzRepository.Repository;

// Configure services
var services = new ServiceCollection();
string connectionString = "Host=localhost;Database=mydb;Username=postgres;Password=postgres;";
services.AddKpzRepositoryPostgreSqlFactory(connectionString);
var serviceProvider = services.BuildServiceProvider();

// Resolve factory and create repository
var factory = serviceProvider.GetRequiredService<IKpzRepositoryFactory>();
var repository = factory.GetBaseRepository<long, Product>();

// Use repository
var product = new Product
{
    name = "Smartphone",
    price = 699.99m,
    quantity = 100,
    is_active = true,
    created_at = DateTime.UtcNow
};

await repository.AddAsync(product);

// Get product by ID
var retrieved = await repository.GetAsync(product.id);
Console.WriteLine($"Retrieved: {retrieved?.name}");

// Cleanup
repository.Dispose();
```

### 3. Common Operations

```csharp
// Get by ID
var product = repository.Get(1);
var productAsync = await repository.GetAsync(1);

// Get all
var allProducts = repository.GetAll();
var allProductsAsync = await repository.GetAllAsync();

// Get all with ordering (use snake_case column names)
var orderedProducts = repository.GetAllOrderBy("price", desc: true);

// Search with LIKE (use snake_case column names)
var searchResults = repository.GetEntitiesLike("name", "Laptop");

// Count
long count = repository.Count();
long countAsync = await repository.CountAsync();

// Check existence
bool exists = repository.Exists(1);
bool existsAsync = await repository.ExistsAsync(1);

// Check if empty
bool isEmpty = repository.IsEmpty();

// Get min/max IDs
var minId = repository.GetMinId();
var maxId = repository.GetMaxId();

// Get min/max entities
var minEntity = repository.GetMinEntity();
var maxEntity = repository.GetMaxEntity();

// Add multiple entities (use transactions!)
var products = new List<Product> { product1, product2, product3 };
var transaction = repository.BeginTransaction();
long insertedCount = repository.AddRange(products, transaction);
transaction.Commit();

// Delete all
repository.DeleteAll();

// Execute custom SQL
int rowsAffected = repository.ExecuteQuery(
    "UPDATE products SET is_active = false WHERE price > @MaxPrice",
    new { MaxPrice = 1000 }
);
```

### 4. String Primary Keys Usage

For entities with string-based primary keys (like UUIDs, custom codes, or natural keys), use the `[ExplicitKey]` attribute instead of `[Key]`. You must manually set the ID value before inserting.

#### Define Entity with String Primary Key

```csharp
using Dapper.Contrib.Extensions;
using KpzRepository.Model;

[Table("sessions")]
public class Session : BaseEntity<string>
{
    [ExplicitKey]  // Use ExplicitKey for string/UUID primary keys
    public string id { get; set; } = null!;

    public string user_id { get; set; } = null!;
    public DateTime created_at { get; set; }
    public DateTime expires_at { get; set; }
    public bool is_active { get; set; }
    public string? ip_address { get; set; }
    public string? user_agent { get; set; }
}
```

**Corresponding PostgreSQL Table:**

```sql
CREATE TABLE sessions (
    id VARCHAR(50) PRIMARY KEY,  -- or UUID type
    user_id VARCHAR(50) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    expires_at TIMESTAMP NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    ip_address VARCHAR(45),
    user_agent TEXT
);
```

#### Create and Use Repository

```csharp
// Get repository for string-based entity
IKpzRepository<string, Session> sessionRepository = factory.GetBaseRepository<string, Session>();

// Create new session - MUST set the id manually
var session = new Session
{
    id = Guid.NewGuid().ToString("N"),  // Generate unique ID
    user_id = "user_12345",
    created_at = DateTime.UtcNow,
    expires_at = DateTime.UtcNow.AddHours(24),
    is_active = true,
    ip_address = "192.168.1.100",
    user_agent = "Mozilla/5.0..."
};

// Add session
sessionRepository.Add(session);
Console.WriteLine($"Session created with ID: {session.id}");

// Get session by string ID
var retrievedSession = sessionRepository.Get(session.id);
if (retrievedSession != null)
{
    Console.WriteLine($"Retrieved session for user: {retrievedSession.user_id}");
}
```

#### Important Notes for String Primary Keys

1. **Always Set ID Manually** - Unlike auto-increment keys, you must set the `id` property before calling `Add()`
2. **Use [ExplicitKey]** - Required attribute for non-auto-increment keys
3. **Ensure Uniqueness** - Your ID generation logic must guarantee unique values
4. **Consider Performance** - String keys are slower than integer keys for indexing
5. **Max Length** - Define appropriate column length in database (e.g., `VARCHAR(50)`) or use `UUID` type
6. **Collation** - PostgreSQL is case-sensitive by default for `VARCHAR` comparisons

### 5. Transaction Management

```csharp
var repository = factory.GetBaseRepository<long, Product>();

// Start transaction
var transaction = repository.BeginTransaction();

try
{
    // Perform multiple operations
    repository.Add(product1, transaction);
    repository.Add(product2, transaction);
    repository.Update(product3, transaction);

    // Commit if all operations succeed
    transaction.Commit();
}
catch (Exception)
{
    // Rollback on error
    transaction.Rollback();
    throw;
}
finally
{
    transaction.Dispose();
}
```

## Best Practices

1. **Use snake_case Everywhere** - PostgreSQL requires snake_case for proper mapping with Dapper.Contrib. Use it for table names, column names, and C# property names.

2. **Use Transactions for Batch Operations** - When adding or updating multiple entities, always use transactions to ensure data consistency and improve performance.

3. **Dispose Resources** - Always dispose repositories and transactions when done:
   ```csharp
   using var repository = factory.GetBaseRepository<long, Product>();
   // Use repository
   ```

4. **Async/Await** - Use async methods for I/O-bound operations:
   ```csharp
   await repository.AddAsync(entity);
   var entities = await repository.GetAllAsync();
   ```

5. **Connection Management** - The repository manages connections automatically, but you can manually control them if needed:
   ```csharp
   repository.OpenConnection();
   // Perform operations
   repository.CloseConnection();
   ```

6. **Custom Queries** - Use `ExecuteQuery` for custom SQL when needed:
   ```csharp
   var sql = "DELETE FROM products WHERE created_at < @Date";
   repository.ExecuteQuery(sql, new { Date = DateTime.UtcNow.AddYears(-1) });
   ```

7. **JSONB Support** - PostgreSQL's JSONB type is automatically handled. Store JSON as strings in your entities:
   ```csharp
   public string? preferences { get; set; }  // Maps to JSONB column
   ```

8. **Use TIMESTAMP for Dates** - Prefer `TIMESTAMP` or `TIMESTAMPTZ` over `DATE` for better precision.

9. **Leverage PostgreSQL Arrays** - You can use arrays in PostgreSQL, but handle them as delimited strings or JSON in your entities.

## Entity Attributes

- **`[Table("table_name")]`** - Specify custom table name (use snake_case)
- **`[Key]`** - Auto-increment primary key (SERIAL, BIGSERIAL)
- **`[ExplicitKey]`** - Manual primary key (VARCHAR, UUID, or manually incremented INT)
- **`[Write(false)]`** - Exclude property from INSERT/UPDATE operations
- **`[Computed]`** - Exclude from INSERT/UPDATE (for computed/generated columns)

## Repository Interface Overview

The `IKpzRepository<TKey, TEntity>` interface provides:

### Connection Management
- `Connection` - Get the database connection
- `OpenConnection()` / `CloseConnection()` - Manual connection control
- `IsConnected` - Check connection status
- `BeginTransaction()` - Start a new transaction

### CRUD Operations
- `Add()` / `AddAsync()` - Insert single entity
- `AddRange()` / `AddRangeAsync()` - Insert multiple entities
- `Update()` / `UpdateAsync()` - Update entity
- `Delete()` / `DeleteAsync()` - Delete by ID
- `DeleteAll()` - Delete all entities

### Query Operations
- `Get()` / `GetAsync()` - Get by ID
- `GetAll()` / `GetAllAsync()` - Get all entities
- `GetAllOrderBy()` / `GetAllOrderByAsync()` - Get all with ordering
- `GetEntitiesLike()` / `GetEntitiesLikeAsync()` - Search with LIKE
- `GetMinEntity()` / `GetMaxEntity()` - Get min/max entities
- `Count()` / `CountAsync()` - Count entities
- `IsEmpty()` / `IsEmptyAsync()` - Check if table is empty
- `Exists()` / `ExistsAsync()` - Check entity existence

### ID Operations
- `GetLastInsertedId()` - Get last inserted ID
- `GetMinId()` / `GetMaxId()` - Get min/max IDs

### Metadata
- `GetRepositoryTableName()` - Get mapped table name
- `GetRepositoryKeyName()` - Get primary key column name

### Custom SQL
- `ExecuteQuery()` / `ExecuteQueryAsync()` - Execute custom SQL

## Extending Repository with Custom Methods

You can extend repositories with custom domain-specific methods. This is useful when you need specialized queries or business logic that goes beyond basic CRUD operations.

This approach extends the database-specific implementation (`KpzRepositoryPostgreSql`), allowing you to add custom methods while maintaining all base functionality.

**Step 1: Create Custom Repository Interface**

```csharp
using KpzRepository.Repository;
using KpzRepository.Model;

namespace MyApp.Repositories;

/// <summary>
/// Extended repository interface with custom methods for Order entity.
/// </summary>
public interface IOrderRepository : IKpzRepository<long, Order>
{
    /// <summary>
    /// Get orders within a specific date range.
    /// </summary>
    IEnumerable<Order> GetOrdersByDateRange(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, IDbTransaction? transaction = null);

    /// <summary>
    /// Get orders for a specific customer.
    /// </summary>
    IEnumerable<Order> GetOrdersByCustomer(string customerName, IDbTransaction? transaction = null);

    /// <summary>
    /// Get total revenue for a date range.
    /// </summary>
    decimal GetTotalRevenue(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, IDbTransaction? transaction = null);

    /// <summary>
    /// Get unpaid orders.
    /// </summary>
    IEnumerable<Order> GetUnpaidOrders(IDbTransaction? transaction = null);
}
```

**Step 2: Implement Custom Repository**

```csharp
using Dapper;
using KpzRepository.Model;
using KpzRepository.PostgreSql.Repository;
using System.Data;

namespace MyApp.Repositories;

/// <summary>
/// Custom PostgreSQL repository for Order entity with specialized methods.
/// Note: All SQL queries use snake_case column names to match PostgreSQL conventions.
/// </summary>
public class OrderRepository : KpzRepositoryPostgreSql<long, Order>, IOrderRepository
{
    public OrderRepository(IDbConnection connection) : base(connection)
    {
    }

    public IEnumerable<Order> GetOrdersByDateRange(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            var sql = @"
                SELECT * FROM orders 
                WHERE (@DateFrom IS NULL OR order_date >= @DateFrom)
                  AND (@DateTo IS NULL OR order_date <= @DateTo)
                ORDER BY order_date DESC";

            return Connection!.Query<Order>(sql, new { DateFrom = dateFrom, DateTo = dateTo }, transaction);
        }
        return Enumerable.Empty<Order>();
    }

    public IEnumerable<Order> GetOrdersByCustomer(string customerName, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            var sql = @"
                SELECT * FROM orders 
                WHERE customer_name ILIKE @CustomerName
                ORDER BY order_date DESC";

            return Connection!.Query<Order>(sql, new { CustomerName = $"%{customerName}%" }, transaction);
        }
        return Enumerable.Empty<Order>();
    }

    public decimal GetTotalRevenue(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            var sql = @"
                SELECT COALESCE(SUM(total_amount), 0) 
                FROM orders 
                WHERE is_paid = true
                  AND (@DateFrom IS NULL OR order_date >= @DateFrom)
                  AND (@DateTo IS NULL OR order_date <= @DateTo)";

            return Connection!.ExecuteScalar<decimal>(sql, new { DateFrom = dateFrom, DateTo = dateTo }, transaction);
        }
        return 0;
    }

    public IEnumerable<Order> GetUnpaidOrders(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            var sql = @"
                SELECT * FROM orders 
                WHERE is_paid = false
                ORDER BY order_date DESC";

            return Connection!.Query<Order>(sql, null, transaction);
        }
        return Enumerable.Empty<Order>();
    }
}
```

**Step 3: Create Custom Factory**

```csharp
using KpzRepository.Factory;
using KpzRepository.Model;
using KpzRepository.Repository;
using KpzRepository.PostgreSql.Factory;
using Npgsql;
using MyApp.Repositories;

namespace MyApp.Factories;

/// <summary>
/// Custom factory that creates specialized repositories.
/// </summary>
public class CustomRepositoryFactory : KpzRepositoryPostgreSqlFactory
{
    public CustomRepositoryFactory(string connectionString) : base(connectionString)
    {
    }

    /// <summary>
    /// Get the custom Order repository with extended methods.
    /// </summary>
    public IOrderRepository GetOrderRepository()
    {
        return new OrderRepository(GetNewConnection(ConnectionString));
    }

    // You can add more specialized repository methods here
    // public IProductRepository GetProductRepository() { ... }
}
```

**Step 4: Usage Example**

```csharp
using MyApp.Factories;
using MyApp.Repositories;

// Create custom factory
string connectionString = "Host=localhost;Database=mydb;Username=postgres;Password=postgres;";
var factory = new CustomRepositoryFactory(connectionString);

// Get custom repository with extended methods
var orderRepository = factory.GetOrderRepository();

// Use base repository methods
var allOrders = orderRepository.GetAll();
var order = orderRepository.Get(1);
orderRepository.Add(new Order { /* ... */ });

// Use custom methods
var recentOrders = orderRepository.GetOrdersByDateRange(
    DateTimeOffset.Now.AddMonths(-1), 
    DateTimeOffset.Now
);

var customerOrders = orderRepository.GetOrdersByCustomer("John Doe");

var revenue = orderRepository.GetTotalRevenue(
    new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
    new DateTimeOffset(2024, 12, 31, 23, 59, 59, TimeSpan.Zero)
);

var unpaidOrders = orderRepository.GetUnpaidOrders();

Console.WriteLine($"Total Revenue: ${revenue:N2}");
Console.WriteLine($"Unpaid Orders: {unpaidOrders.Count()}");
```

### Best Practices for Custom Repositories

1. **Keep Methods Focused** - Each custom method should have a single, clear purpose
2. **Use snake_case in SQL** - All SQL queries must use snake_case column names
3. **Use ILIKE for Case-Insensitive Search** - PostgreSQL's `ILIKE` is better than `LIKE` for text searches
4. **Use COALESCE Instead of ISNULL** - PostgreSQL uses `COALESCE` instead of SQL Server's `ISNULL`
5. **Use Transactions** - Always support optional transaction parameters for consistency
6. **Handle Connections** - Always check `OpenConnection()` before executing queries
7. **Return Empty Collections** - Return `Enumerable.Empty<T>()` instead of `null` for failed queries
8. **Use Parameterized Queries** - Always use Dapper parameters to prevent SQL injection
9. **Document Your Methods** - Add XML documentation for all custom methods
10. **Test Thoroughly** - Write unit tests for each custom method
11. **Consider Async** - Provide async versions of custom methods for better scalability

### Summary

Extending KpzRepository with custom methods allows you to:

- ✅ Add domain-specific query methods
- ✅ Encapsulate complex business logic
- ✅ Maintain separation of concerns
- ✅ Keep all repository benefits (transactions, connection management, etc.)
- ✅ Use dependency injection seamlessly
- ✅ Write testable, maintainable code
- ✅ Leverage PostgreSQL-specific features (JSONB, arrays, full-text search, etc.)

## Implementing Custom Database Provider

KpzRepository is designed to be extensible. You can implement support for any database by following these steps:

### Architecture Overview

The repository pattern consists of three main components:

1. **Repository Implementation** - Inherits from `KpzRepository<TKey, TEntity>` and overrides database-specific methods
2. **Factory** - Implements `IKpzRepositoryFactory` to create repository instances
3. **Dependency Injection Extension** - Optional helper for registering the factory

### Step-by-Step Guide

Let's create a custom implementation for **MySQL** as an example.

#### 1. Create a New Class Library Project

```bash
dotnet new classlib -n KpzRepository.MySql
dotnet add KpzRepository.MySql package MySql.Data
dotnet add KpzRepository.MySql reference KpzRepository
```

#### 2. Implement the Repository Class

Create `Repository/KpzRepositoryMySql.cs`:

```csharp
using Dapper;
using KpzRepository.Model;
using KpzRepository.Repository;
using System.Data;

namespace KpzRepository.MySql.Repository;

/// <summary>
/// MySQL implementation of the repository.
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class KpzRepositoryMySql<TKey, TEntity> : KpzRepository<TKey, TEntity>, IKpzRepository<TKey, TEntity>
    where TEntity : BaseEntity<TKey>, new()
{
    public KpzRepositoryMySql(IDbConnection connection) : base(connection)
    {
    }

    /// <summary>
    /// Override this method to implement database-specific logic for retrieving the last inserted ID.
    /// This is the main method that differs between database providers.
    /// </summary>
    public override TKey GetLastInsertedId(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            // MySQL uses LAST_INSERT_ID() to get the last auto-increment value
            string sql = "SELECT LAST_INSERT_ID() AS LastInsertedId";
            var result = Connection!.ExecuteScalar<TKey>(sql, null, transaction);
            if (result != null)
            {
                return result;
            }
        }
        return default!;
    }
}
```

**Key Points:**
- Inherit from `KpzRepository<TKey, TEntity>`
- Implement `IKpzRepository<TKey, TEntity>`
- Override `GetLastInsertedId()` with database-specific SQL
- The base class handles all other CRUD operations

#### 3. Create the Factory

Create `Factory/KpzRepositoryMySqlFactory.cs`:

```csharp
using KpzRepository.Factory;
using KpzRepository.Model;
using KpzRepository.MySql.Repository;
using KpzRepository.Repository;
using MySql.Data.MySqlClient;

namespace KpzRepository.MySql.Factory;

/// <summary>
/// Factory class for creating MySQL repositories.
/// </summary>
public class KpzRepositoryMySqlFactory : IKpzRepositoryFactory
{
    public KpzRepositoryMySqlFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    /// <summary>
    /// Creates a repository instance for the specified entity type.
    /// </summary>
    public IKpzRepository<TKey, TEntity> GetBaseRepository<TKey, TEntity>()
        where TEntity : BaseEntity<TKey>, new()
    {
        return new KpzRepositoryMySql<TKey, TEntity>(GetNewConnection(ConnectionString));
    }

    /// <summary>
    /// Creates a new database connection. Override this if you need custom connection logic.
    /// </summary>
    protected virtual MySqlConnection GetNewConnection(string connectionString)
    {
        return new MySqlConnection(connectionString);
    }

    protected virtual string ConnectionString { get; set; } = string.Empty;
}
```

#### 4. Add Dependency Injection Support (Optional)

Create `DependencyInjection.cs`:

```csharp
using KpzRepository.Factory;
using KpzRepository.MySql.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace KpzRepository.MySql;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the MySQL repository factory in the DI container.
    /// </summary>
    public static IServiceCollection AddKpzRepositoryMySqlFactory(
        this IServiceCollection services, 
        string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        var repoFactoryDescriptor = new ServiceDescriptor(
            typeof(IKpzRepositoryFactory),
            provider => new KpzRepositoryMySqlFactory(connectionString),
            ServiceLifetime.Transient);

        services.Add(repoFactoryDescriptor);

        return services;
    }
}
```

#### 5. Usage Example

Now you can use your custom MySQL implementation:

```csharp
using KpzRepository.MySql.Factory;
using KpzRepository.Factory;
using KpzRepository.Repository;

// Direct usage
string connectionString = "Server=localhost;Database=mydb;Uid=root;Pwd=password;";
IKpzRepositoryFactory factory = new KpzRepositoryMySqlFactory(connectionString);
IKpzRepository<long, Product> repository = factory.GetBaseRepository<long, Product>();

// Or with Dependency Injection
services.AddKpzRepositoryMySqlFactory(connectionString);
```

### Advanced Customization

#### Custom Type Handlers (e.g., for JSONB in PostgreSQL)

If your database requires special type handling, you can register Dapper type handlers:

```csharp
using Dapper;
using System.Data;
using System.Text.Json;

public class JsonTypeHandler : SqlMapper.TypeHandler<string>
{
    public override void SetValue(IDbDataParameter parameter, string? value)
    {
        parameter.Value = value ?? (object)DBNull.Value;
    }

    public override string Parse(object value)
    {
        return value?.ToString() ?? string.Empty;
    }
}

// Register in your DependencyInjection or Factory
SqlMapper.AddTypeHandler(new JsonTypeHandler());
```

#### Override Additional Methods

If you need to customize other behaviors, you can override additional virtual methods from the base `KpzRepository<TKey, TEntity>` class:

```csharp
public override bool Add(TEntity entity, IDbTransaction? transaction = null)
{
    // Custom logic before insert
    entity.CreatedAt = DateTime.UtcNow;

    // Call base implementation
    var result = base.Add(entity, transaction);

    // Custom logic after insert
    LogInsert(entity);

    return result;
}
```

### Key Considerations

1. **Connection Type** - Use the appropriate ADO.NET provider for your database
2. **Last Insert ID** - This is the primary method you need to implement
3. **SQL Dialect** - Most queries are handled by Dapper.Contrib, but be aware of any SQL syntax differences
4. **Type Mapping** - Register custom type handlers if needed (e.g., JSON, arrays, enums)
5. **Transaction Support** - The base implementation handles transactions, but test thoroughly with your database
6. **Naming Conventions** - Consider your database's naming conventions (PascalCase vs snake_case)

### Contributing Your Implementation

If you create a provider for another database, consider contributing it back to the KpzRepository ecosystem! Submit a pull request or publish your own NuGet package.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is open source. Please check the license file for more details.

## Links

- **GitHub**: [https://github.com/malicone/KpzRepository](https://github.com/malicone/KpzRepository)
- **Website**: [kpzrepository.com](http://kpzrepository.com)

## Author

Maxim Mihaluk

---

**Built with ❤️ using .NET 8, Dapper, Dapper.Contrib, and Npgsql**
