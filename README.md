# KpzRepository

A lightweight and flexible repository pattern implementation for .NET 8, providing a unified interface for database operations across SQL Server, PostgreSQL, and SQLite. Built on top of Dapper and Dapper.Contrib, KpzRepository simplifies data access while maintaining performance and flexibility.

![KpzRepository](data:image/png;base64,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)

## Table of Contents

- [Installation](#installation)
- [Quick Start](#quick-start)
- [PostgreSQL usage](#postgresql-usage)
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
# Choose your database provider:
dotnet add package KpzRepository.SqlServer
dotnet add package KpzRepository.PostgreSql
dotnet add package KpzRepository.Sqlite
```

## Quick Start
### For SQL Server

### 1. Define Your Entity

Create a class that inherits from `BaseEntity<TKey>`:

```csharp
using Dapper.Contrib.Extensions;
using KpzRepository.Model;

[Table("Products")]// You can omit this if your class name matches the table name (for example, "Product" class would map to "Product" table).
public class Product : BaseEntity<long>
{
    [Key]// You need to specify [Key] for auto-incrementing primary keys (int, long). For string or Guid keys, use [ExplicitKey] and set the value manually.
    public long Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### 2. Create Repository Instance

#### Direct Creation

```csharp
using KpzRepository.Factory;
using KpzRepository.Repository;
using KpzRepository.SqlServer.Factory;

// Create factory
string connectionString = "Server=localhost;Database=MyDb;Trusted_Connection=True;";
IKpzRepositoryFactory factory = new KpzRepositorySqlServerFactory(connectionString);

// Get repository for your entity
IKpzRepository<long, Product> repository = factory.GetBaseRepository<long, Product>();

// Add a product
var product = new Product
{
    Name = "Laptop",
    Description = "High-performance laptop",
    Price = 999.99m,
    Quantity = 50,
    IsActive = true,
    CreatedAt = DateTime.UtcNow
};

repository.Add(product);
Console.WriteLine($"Product added with ID: {product.Id}");

// Get all products
var products = repository.GetAll();
foreach (var p in products)
{
    Console.WriteLine($"{p.Name} - ${p.Price}");
}

// Update product
product.Price = 899.99m;
repository.Update(product);

// Delete product
repository.Delete(product.Id);

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
string connectionString = "Server=localhost;Database=MyDb;Trusted_Connection=True;";
services.AddKpzRepositorySqlServerFactory(connectionString);
var serviceProvider = services.BuildServiceProvider();

// Resolve factory and create repository
var factory = serviceProvider.GetRequiredService<IKpzRepositoryFactory>();
var repository = factory.GetBaseRepository<long, Product>();

// Use repository
var product = new Product
{
    Name = "Smartphone",
    Price = 699.99m,
    Quantity = 100,
    IsActive = true,
    CreatedAt = DateTime.UtcNow
};

await repository.AddAsync(product);

// Get product by ID
var retrieved = await repository.GetAsync(product.Id);
Console.WriteLine($"Retrieved: {retrieved?.Name}");

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

// Get all with ordering
var orderedProducts = repository.GetAllOrderBy("Price", desc: true);

// Search with LIKE
var searchResults = repository.GetEntitiesLike("Name", "Laptop");

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
    "UPDATE Products SET IsActive = 0 WHERE Price > @MaxPrice",
    new { MaxPrice = 1000 }
);
```

### 4. String Primary Keys Usage

For entities with string-based primary keys (like GUIDs, custom codes, or natural keys), use the `[ExplicitKey]` attribute instead of `[Key]`. You must manually set the ID value before inserting.

#### Define Entity with String Primary Key

```csharp
using Dapper.Contrib.Extensions;
using KpzRepository.Model;

[Table("Sessions")]
public class Session : BaseEntity<string>
{
    [ExplicitKey]  // Use ExplicitKey for string/Guid primary keys
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}
```

#### Create and Use Repository

```csharp
// Get repository for string-based entity
IKpzRepository<string, Session> sessionRepository = factory.GetBaseRepository<string, Session>();

// Create new session - MUST set the Id manually
var session = new Session
{
    Id = Guid.NewGuid().ToString("N"),  // Generate unique ID
    UserId = "user_12345",
    CreatedAt = DateTime.UtcNow,
    ExpiresAt = DateTime.UtcNow.AddHours(24),
    IsActive = true,
    IpAddress = "192.168.1.100",
    UserAgent = "Mozilla/5.0..."
};

// Add session
sessionRepository.Add(session);
Console.WriteLine($"Session created with ID: {session.Id}");

// Get session by string ID
var retrievedSession = sessionRepository.Get(session.Id);
if (retrievedSession != null)
{
    Console.WriteLine($"Retrieved session for user: {retrievedSession.UserId}");
}
```

#### Important Notes for String Primary Keys

1. **Always Set ID Manually** - Unlike auto-increment keys, you must set the `Id` property before calling `Add()`
2. **Use [ExplicitKey]** - Required attribute for non-auto-increment keys
3. **Ensure Uniqueness** - Your ID generation logic must guarantee unique values
4. **Consider Performance** - String keys are slower than integer keys for indexing
5. **Max Length** - Define appropriate column length in database (e.g., `VARCHAR(50)`)
6. **Collation** - Be aware of case-sensitivity based on database collation settings

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

## PostgreSQL usage

### ⚠️ Important: snake_case is Required for PostgreSQL

**Dapper.Contrib cannot work correctly with PascalCase/camelCase property names in PostgreSQL.** PostgreSQL treats unquoted identifiers as lowercase, which causes mismatches when Dapper.Contrib tries to map PascalCase properties to lowercase columns.

### Entity Declaration with snake_case

```csharp
using Dapper.Contrib.Extensions;
using KpzRepository.Model;

[Table("user_profiles")]  // Table name in snake_case
public class UserProfile : BaseEntity<long>
{
    [Key]
    public long id { get; set; }  // snake_case properties to match PostgreSQL columns

    public string user_name { get; set; } = null!;
    public string email_address { get; set; } = null!;
    public string? phone_number { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public bool is_active { get; set; }

    // PostgreSQL specific: JSONB support
    public string? preferences { get; set; }  // Store JSON data as JSONB
}
```

### Corresponding PostgreSQL Table

```sql
CREATE TABLE user_profiles (
    id BIGSERIAL PRIMARY KEY,
    user_name VARCHAR(100) NOT NULL,
    email_address VARCHAR(255) NOT NULL,
    phone_number VARCHAR(20),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP,
    is_active BOOLEAN NOT NULL DEFAULT true,
    preferences JSONB
);
```

## Best Practices

1. **Use Transactions for Batch Operations** - When adding or updating multiple entities, always use transactions to ensure data consistency and improve performance.

2. **Dispose Resources** - Always dispose repositories and transactions when done:
   ```csharp
   using var repository = factory.GetBaseRepository<long, Product>();
   // Use repository
   ```

3. **Async/Await** - Use async methods for I/O-bound operations:
   ```csharp
   await repository.AddAsync(entity);
   var entities = await repository.GetAllAsync();
   ```

4. **Connection Management** - The repository manages connections automatically, but you can manually control them if needed:
   ```csharp
   repository.OpenConnection();
   // Perform operations
   repository.CloseConnection();
   ```

5. **Custom Queries** - Use `ExecuteQuery` for custom SQL when needed:
   ```csharp
   var sql = "DELETE FROM Products WHERE CreatedAt < @Date";
   repository.ExecuteQuery(sql, new { Date = DateTime.UtcNow.AddYears(-1) });
   ```

## Entity Attributes

- **`[Table("table_name")]`** - Specify custom table name
- **`[Key]`** - Auto-increment primary key (int, long)
- **`[ExplicitKey]`** - Manual primary key (string, Guid or int incremented manually)
- **`[Write(false)]`** - Exclude property from INSERT/UPDATE operations
- **`[Computed]`** - Exclude from INSERT/UPDATE (for computed columns)

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

This approach extends the database-specific implementation (e.g., `KpzRepositorySqlServer`), allowing you to add custom methods while maintaining all base functionality.

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
using KpzRepository.SqlServer.Repository;
using System.Data;

namespace MyApp.Repositories;

/// <summary>
/// Custom SQL Server repository for Order entity with specialized methods.
/// </summary>
public class OrderRepository : KpzRepositorySqlServer<long, Order>, IOrderRepository
{
    public OrderRepository(IDbConnection connection) : base(connection)
    {
    }

    public IEnumerable<Order> GetOrdersByDateRange(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            var sql = @"
                SELECT * FROM Orders 
                WHERE (@DateFrom IS NULL OR OrderDate >= @DateFrom)
                  AND (@DateTo IS NULL OR OrderDate <= @DateTo)
                ORDER BY OrderDate DESC";

            return Connection!.Query<Order>(sql, new { DateFrom = dateFrom, DateTo = dateTo }, transaction);
        }
        return Enumerable.Empty<Order>();
    }

    public IEnumerable<Order> GetOrdersByCustomer(string customerName, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            var sql = @"
                SELECT * FROM Orders 
                WHERE CustomerName LIKE @CustomerName
                ORDER BY OrderDate DESC";

            return Connection!.Query<Order>(sql, new { CustomerName = $"%{customerName}%" }, transaction);
        }
        return Enumerable.Empty<Order>();
    }

    public decimal GetTotalRevenue(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            var sql = @"
                SELECT ISNULL(SUM(TotalAmount), 0) 
                FROM Orders 
                WHERE IsPaid = 1
                  AND (@DateFrom IS NULL OR OrderDate >= @DateFrom)
                  AND (@DateTo IS NULL OR OrderDate <= @DateTo)";

            return Connection!.ExecuteScalar<decimal>(sql, new { DateFrom = dateFrom, DateTo = dateTo }, transaction);
        }
        return 0;
    }

    public IEnumerable<Order> GetUnpaidOrders(IDbTransaction? transaction = null)
    {
        if (OpenConnection())
        {
            var sql = @"
                SELECT * FROM Orders 
                WHERE IsPaid = 0
                ORDER BY OrderDate DESC";

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
using KpzRepository.SqlServer.Factory;
using Microsoft.Data.SqlClient;
using MyApp.Repositories;

namespace MyApp.Factories;

/// <summary>
/// Custom factory that creates specialized repositories.
/// </summary>
public class CustomRepositoryFactory : KpzRepositorySqlServerFactory
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
string connectionString = "Server=localhost;Database=MyDb;Trusted_Connection=True;";
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
2. **Use Transactions** - Always support optional transaction parameters for consistency
3. **Handle Connections** - Always check `OpenConnection()` before executing queries
4. **Return Empty Collections** - Return `Enumerable.Empty<T>()` instead of `null` for failed queries
5. **Use Parameterized Queries** - Always use Dapper parameters to prevent SQL injection
6. **Document Your Methods** - Add XML documentation for all custom methods
7. **Test Thoroughly** - Write unit tests for each custom method
8. **Consider Async** - Provide async versions of custom methods for better scalability

### Summary

Extending KpzRepository with custom methods allows you to:

- ✅ Add domain-specific query methods
- ✅ Encapsulate complex business logic
- ✅ Maintain separation of concerns
- ✅ Keep all repository benefits (transactions, connection management, etc.)
- ✅ Use dependency injection seamlessly
- ✅ Write testable, maintainable code

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

**Built with ❤️ using .NET 8, Dapper, and Dapper.Contrib**  
*Built with Visual Studio 2026 Insiders [11819.209], Class library template, targeting .NET8.0, C# 12.*