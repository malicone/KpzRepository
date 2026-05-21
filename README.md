# KpzRepository

![KpzRepository](images/KpzRepository_lib_icon.png)

A lightweight and flexible repository pattern implementation for .NET 8, providing a unified interface for database operations across SQL Server, PostgreSQL, and SQLite. Built on top of Dapper and Dapper.Contrib, KpzRepository simplifies data access while maintaining performance and flexibility.

## Features

- 🎯 **Unified Interface** - Single repository interface works with SQL Server, PostgreSQL, and SQLite
- 🚀 **High Performance** - Built on Dapper for optimal database performance
- 💉 **Dependency Injection** - First-class support for .NET dependency injection
- 🔄 **Async Support** - Full async/await support for all operations
- 🔐 **Transaction Support** - Built-in transaction management
- 📦 **Generic Repository Pattern** - Type-safe CRUD operations with strong typing
- 🎨 **Flexible Key Types** - Support for `int`, `long`, `string`, `Guid`, and more
- 🛠️ **Extensible** - Easy to extend with custom queries and operations

## Installation

Install only the database provider you need - the core package **KpzRepository** is included automatically:

```bash
# Choose your database provider:
dotnet add package KpzRepository.SqlServer
dotnet add package KpzRepository.PostgreSql
dotnet add package KpzRepository.Sqlite
```

## Quick Start

### 1. Define Your Entity

Create a class that inherits from `BaseEntity<TKey>`:

```csharp
using Dapper.Contrib.Extensions;
using KpzRepository.Model;

[Table("Products")]
public class Product : BaseEntity<long>
{
    [Key]
    public long Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### 2. Usage Examples

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

## Database-Specific Configuration

### SQL Server

```csharp
using Microsoft.Extensions.DependencyInjection;
using KpzRepository;
using KpzRepository.Factory;

// Connection string
string connectionString = "Server=localhost;Database=MyDatabase;Trusted_Connection=True;";

// Option 1: Direct creation
var factory = new KpzRepositorySqlServerFactory(connectionString);
var repository = factory.GetBaseRepository<long, Product>();

// Option 2: Dependency Injection
var services = new ServiceCollection();
services.AddKpzRepositorySqlServerFactory(connectionString);
var serviceProvider = services.BuildServiceProvider();

var factoryFromDI = serviceProvider.GetRequiredService<IKpzRepositoryFactory>();
var repositoryFromDI = factoryFromDI.GetBaseRepository<long, Product>();
```

#### SQL Server Entity Example

```csharp
using Dapper.Contrib.Extensions;
using KpzRepository.Model;

[Table("Orders")]
public class Order : BaseEntity<long>
{
    [Key]
    public long Id { get; set; }

    public string OrderNumber { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string CustomerName { get; set; } = null!;
    public string? ShippingAddress { get; set; }
    public bool IsPaid { get; set; }

    // SQL Server specific: timestamp for concurrency
    [Write(false)]
    public byte[] RowVersion { get; set; } = null!;
}

// Usage
var factory = new KpzRepositorySqlServerFactory(connectionString);
var orderRepository = factory.GetBaseRepository<long, Order>();

var order = new Order
{
    OrderNumber = "ORD-001",
    OrderDate = DateTime.UtcNow,
    TotalAmount = 1250.50m,
    CustomerName = "John Doe",
    IsPaid = false
};

orderRepository.Add(order);
```

### PostgreSQL

```csharp
using Microsoft.Extensions.DependencyInjection;
using KpzRepository.PostgreSql;
using KpzRepository.Factory;

// Connection string
string connectionString = "Host=localhost;Database=mydb;Username=postgres;Password=secret";

// Option 1: Direct creation
var factory = new KpzRepositoryPostgreSqlFactory(connectionString);
var repository = factory.GetBaseRepository<long, Product>();

// Option 2: Dependency Injection
var services = new ServiceCollection();
services.AddKpzRepositoryPostgreSqlFactory(connectionString);
var serviceProvider = services.BuildServiceProvider();

var factoryFromDI = serviceProvider.GetRequiredService<IKpzRepositoryFactory>();
var repositoryFromDI = factoryFromDI.GetBaseRepository<long, Product>();
```

#### PostgreSQL Entity Example with Snake Case

PostgreSQL conventionally uses snake_case naming. You can map C# properties to snake_case columns:

```csharp
using Dapper.Contrib.Extensions;
using KpzRepository.Model;

[Table("user_profiles")]
public class UserProfile : BaseEntity<long>
{
    [Key]
    public long id { get; set; }

    public string user_name { get; set; } = null!;
    public string email_address { get; set; } = null!;
    public string? phone_number { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public bool is_active { get; set; }

    // PostgreSQL specific: JSONB support
    public string? preferences { get; set; } // Store JSON data
}

// Usage
var factory = new KpzRepositoryPostgreSqlFactory(connectionString);
var userRepository = factory.GetBaseRepository<long, UserProfile>();

var user = new UserProfile
{
    user_name = "john_doe",
    email_address = "john@example.com",
    phone_number = "+1234567890",
    created_at = DateTime.UtcNow,
    is_active = true,
    preferences = "{\"theme\":\"dark\",\"language\":\"en\"}"
};

userRepository.Add(user);

// Search by username
var users = userRepository.GetEntitiesLike("user_name", "john");
```

### SQLite

```csharp
using Microsoft.Extensions.DependencyInjection;
using KpzRepository.Sqlite;
using KpzRepository.Factory;

// Connection string (file-based)
string connectionString = "Data Source=myapp.db;Cache=Shared";

// Option 1: Direct creation
var factory = new KpzRepositorySqliteFactory(connectionString);
var repository = factory.GetBaseRepository<long, Product>();

// Option 2: Dependency Injection
var services = new ServiceCollection();
services.AddKpzRepositorySqliteFactory(connectionString);
var serviceProvider = services.BuildServiceProvider();

var factoryFromDI = serviceProvider.GetRequiredService<IKpzRepositoryFactory>();
var repositoryFromDI = factoryFromDI.GetBaseRepository<long, Product>();
```

#### SQLite Entity Example

```csharp
using Dapper.Contrib.Extensions;
using KpzRepository.Model;

[Table("tasks")]
public class Task : BaseEntity<long>
{
    [Key]
    public long id { get; set; }

    public string title { get; set; } = null!;
    public string? description { get; set; }
    public int priority { get; set; }
    public bool is_completed { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? completed_at { get; set; }
    public string? category { get; set; }
}

// Usage
var factory = new KpzRepositorySqliteFactory(connectionString);
var taskRepository = factory.GetBaseRepository<long, Task>();

var task = new Task
{
    title = "Complete documentation",
    description = "Write comprehensive README",
    priority = 1,
    is_completed = false,
    created_at = DateTime.UtcNow,
    category = "Development"
};

taskRepository.Add(task);
Console.WriteLine($"Task created with ID: {task.id}");

// Get all incomplete tasks ordered by priority
var incompleteTasks = taskRepository
    .GetAll()
    .Where(t => !t.is_completed)
    .OrderBy(t => t.priority);
```

#### SQLite with String Primary Keys

```csharp
using Dapper.Contrib.Extensions;
using KpzRepository.Model;

[Table("sessions")]
public class Session : BaseEntity<string>
{
    [ExplicitKey]
    public string id { get; set; } = null!;

    public string user_id { get; set; } = null!;
    public DateTime created_at { get; set; }
    public DateTime expires_at { get; set; }
    public bool is_active { get; set; }
    public string? ip_address { get; set; }
}

// Usage
var factory = new KpzRepositorySqliteFactory(connectionString);
var sessionRepository = factory.GetBaseRepository<string, Session>();

var session = new Session
{
    id = Guid.NewGuid().ToString("N"),
    user_id = "user123",
    created_at = DateTime.UtcNow,
    expires_at = DateTime.UtcNow.AddHours(24),
    is_active = true,
    ip_address = "192.168.1.1"
};

sessionRepository.Add(session);

// Get session by ID
var retrievedSession = sessionRepository.Get(session.id);
```

## Transaction Management

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
- **`[ExplicitKey]`** - Manual primary key (string, Guid)
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

**Database-Specific SQL for Getting Last Inserted ID:**
- **SQL Server**: `IDENT_CURRENT('tableName')` or `SCOPE_IDENTITY()`
- **PostgreSQL**: `LASTVAL()` or `CURRVAL('sequence_name')`
- **SQLite**: `last_insert_rowid()`
- **MySQL**: `LAST_INSERT_ID()`
- **Oracle**: `sequence_name.CURRVAL`

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

### Testing Your Implementation

Create a test project following the pattern used in existing providers:

```csharp
[TestFixture]
public class MySqlRepositoryTests
{
    private IKpzRepositoryFactory? _factory;
    private IKpzRepository<long, Product>? _repository;

    [OneTimeSetUp]
    public void Setup()
    {
        string connectionString = "Server=localhost;Database=testdb;Uid=root;Pwd=password;";
        _factory = new KpzRepositoryMySqlFactory(connectionString);
        _repository = _factory.GetBaseRepository<long, Product>();
    }

    [Test]
    public void Add_ShouldInsertEntity()
    {
        var product = new Product { Name = "Test Product", Price = 99.99m };
        var result = _repository!.Add(product);

        Assert.That(result, Is.True);
        Assert.That(product.Id, Is.GreaterThan(0));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _repository?.Dispose();
    }
}
```

### Project Structure

Your custom provider project should follow this structure:

```
KpzRepository.MySql/
├── Factory/
│   └── KpzRepositoryMySqlFactory.cs
├── Repository/
│   └── KpzRepositoryMySql.cs
├── DependencyInjection.cs
└── KpzRepository.MySql.csproj

KpzRepository.MySql.Tests/
├── Model/
│   └── Product.cs
├── MySqlRepositoryTests.cs
└── KpzRepository.MySql.Tests.csproj
```

### NuGet Package Configuration

Update your `.csproj` file to create a NuGet package:

```xml
<PropertyGroup>
  <TargetFramework>net8.0</TargetFramework>
  <Title>KpzRepository.MySql</Title>
  <Description>Repository pattern implementation for MySQL</Description>
  <PackageTags>repository-pattern;database;mysql</PackageTags>
  <RepositoryUrl>https://github.com/yourname/KpzRepository</RepositoryUrl>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="MySql.Data" Version="9.0.0" />
</ItemGroup>

<ItemGroup>
  <ProjectReference Include="..\KpzRepository\KpzRepository.csproj" />
</ItemGroup>
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

## Extending Repository with Custom Methods

Beyond implementing database providers, you can extend repositories with custom domain-specific methods. This is useful when you need specialized queries or business logic that goes beyond basic CRUD operations.

### Creating a Custom Repository

There are two approaches to extend the repository:

#### Approach 1: Extend the Database-Specific Repository (Recommended)

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

#### Approach 2: Wrapper/Decorator Pattern

If you don't want to inherit from database-specific implementations, you can use composition:

```csharp
using KpzRepository.Repository;
using Dapper;
using System.Data;

namespace MyApp.Repositories;

/// <summary>
/// Wrapper that adds custom methods while delegating base operations to the underlying repository.
/// </summary>
public class OrderRepositoryWrapper : IOrderRepository
{
    private readonly IKpzRepository<long, Order> _baseRepository;

    public OrderRepositoryWrapper(IKpzRepository<long, Order> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    // Delegate all IKpzRepository methods to base repository
    public IDbConnection? Connection => _baseRepository.Connection;
    public bool OpenConnection() => _baseRepository.OpenConnection();
    public bool CloseConnection() => _baseRepository.CloseConnection();
    public bool IsConnected => _baseRepository.IsConnected;
    public IDbTransaction? BeginTransaction() => _baseRepository.BeginTransaction();
    public string GetRepositoryTableName() => _baseRepository.GetRepositoryTableName();
    public string GetRepositoryKeyName() => _baseRepository.GetRepositoryKeyName();

    public bool Add(Order entity, IDbTransaction? transaction = null) => _baseRepository.Add(entity, transaction);
    public Task<bool> AddAsync(Order entity, IDbTransaction? transaction = null) => _baseRepository.AddAsync(entity, transaction);
    public long AddRange(IEnumerable<Order> entities, IDbTransaction? transaction = null) => _baseRepository.AddRange(entities, transaction);
    public Task<long> AddRangeAsync(IEnumerable<Order> entities, IDbTransaction? transaction = null) => _baseRepository.AddRangeAsync(entities, transaction);

    public bool Update(Order entity, IDbTransaction? transaction = null) => _baseRepository.Update(entity, transaction);
    public Task<bool> UpdateAsync(Order entity, IDbTransaction? transaction = null) => _baseRepository.UpdateAsync(entity, transaction);

    public bool Delete(long id, IDbTransaction? transaction = null) => _baseRepository.Delete(id, transaction);
    public Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null) => _baseRepository.DeleteAsync(id, transaction);
    public bool DeleteAll(IDbTransaction? transaction = null) => _baseRepository.DeleteAll(transaction);

    public Order? Get(long id, IDbTransaction? transaction = null) => _baseRepository.Get(id, transaction);
    public Task<Order?> GetAsync(long id, IDbTransaction? transaction = null) => _baseRepository.GetAsync(id, transaction);
    public IEnumerable<Order> GetAll(IDbTransaction? transaction = null) => _baseRepository.GetAll(transaction);
    public Task<IEnumerable<Order>> GetAllAsync(IDbTransaction? transaction = null) => _baseRepository.GetAllAsync(transaction);

    // ... (implement remaining interface methods)

    public void Dispose() => _baseRepository.Dispose();

    // Custom methods
    public IEnumerable<Order> GetOrdersByDateRange(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, IDbTransaction? transaction = null)
    {
        if (_baseRepository.OpenConnection())
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

    // ... (implement other custom methods)
}
```

### Using Custom Repositories with Dependency Injection

Register your custom repositories in the DI container:

```csharp
using Microsoft.Extensions.DependencyInjection;
using MyApp.Repositories;
using MyApp.Factories;

var services = new ServiceCollection();

// Option 1: Register custom factory
string connectionString = "Server=localhost;Database=MyDb;Trusted_Connection=True;";
services.AddSingleton<CustomRepositoryFactory>(sp => new CustomRepositoryFactory(connectionString));

// Option 2: Register repositories directly
services.AddTransient<IOrderRepository>(sp =>
{
    var factory = sp.GetRequiredService<CustomRepositoryFactory>();
    return factory.GetOrderRepository();
});

var serviceProvider = services.BuildServiceProvider();

// Resolve from DI
var orderRepository = serviceProvider.GetRequiredService<IOrderRepository>();
```

### Using Custom Repositories in ASP.NET Core

```csharp
// Program.cs or Startup.cs
var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Register custom factory
builder.Services.AddScoped<CustomRepositoryFactory>(sp => new CustomRepositoryFactory(connectionString));

// Register custom repositories
builder.Services.AddScoped<IOrderRepository>(sp =>
{
    var factory = sp.GetRequiredService<CustomRepositoryFactory>();
    return factory.GetOrderRepository();
});

var app = builder.Build();

// Controller usage
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrdersController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet("revenue")]
    public IActionResult GetRevenue([FromQuery] DateTimeOffset? from, [FromQuery] DateTimeOffset? to)
    {
        var revenue = _orderRepository.GetTotalRevenue(from, to);
        return Ok(new { Revenue = revenue });
    }

    [HttpGet("unpaid")]
    public IActionResult GetUnpaidOrders()
    {
        var orders = _orderRepository.GetUnpaidOrders();
        return Ok(orders);
    }

    [HttpGet("customer/{name}")]
    public IActionResult GetCustomerOrders(string name)
    {
        var orders = _orderRepository.GetOrdersByCustomer(name);
        return Ok(orders);
    }
}
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

### Testing Custom Repositories

```csharp
using NUnit.Framework;
using MyApp.Repositories;
using MyApp.Factories;

[TestFixture]
public class OrderRepositoryTests
{
    private CustomRepositoryFactory? _factory;
    private IOrderRepository? _repository;

    [OneTimeSetUp]
    public void Setup()
    {
        string connectionString = "Server=localhost;Database=TestDb;Trusted_Connection=True;";
        _factory = new CustomRepositoryFactory(connectionString);
        _repository = _factory.GetOrderRepository();
    }

    [Test]
    public void GetOrdersByDateRange_ShouldReturnOrdersInRange()
    {
        // Arrange
        var dateFrom = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var dateTo = new DateTimeOffset(2024, 12, 31, 23, 59, 59, TimeSpan.Zero);

        // Act
        var orders = _repository!.GetOrdersByDateRange(dateFrom, dateTo);

        // Assert
        Assert.That(orders, Is.Not.Null);
        Assert.That(orders.All(o => o.OrderDate >= dateFrom && o.OrderDate <= dateTo), Is.True);
    }

    [Test]
    public void GetTotalRevenue_ShouldReturnCorrectSum()
    {
        // Arrange
        var dateFrom = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var dateTo = new DateTimeOffset(2024, 12, 31, 23, 59, 59, TimeSpan.Zero);

        // Act
        var revenue = _repository!.GetTotalRevenue(dateFrom, dateTo);

        // Assert
        Assert.That(revenue, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public void GetUnpaidOrders_ShouldReturnOnlyUnpaidOrders()
    {
        // Act
        var orders = _repository!.GetUnpaidOrders();

        // Assert
        Assert.That(orders, Is.Not.Null);
        Assert.That(orders.All(o => !o.IsPaid), Is.True);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _repository?.Dispose();
    }
}
```

### Summary

Extending KpzRepository with custom methods allows you to:

- ✅ Add domain-specific query methods
- ✅ Encapsulate complex business logic
- ✅ Maintain separation of concerns
- ✅ Keep all repository benefits (transactions, connection management, etc.)
- ✅ Use dependency injection seamlessly
- ✅ Write testable, maintainable code

Choose **Approach 1 (Inheritance)** when you want full control and database-specific optimizations. Choose **Approach 2 (Wrapper)** when you need database-agnostic custom repositories or want to avoid tight coupling to specific implementations.

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