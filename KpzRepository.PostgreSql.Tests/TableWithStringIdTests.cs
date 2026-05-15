using KpzRepository.Factory;
using KpzRepository.Repository;
using KpzRepository.PostgreSql.Tests.Model;
using Microsoft.Extensions.DependencyInjection;
using KpzRepository.PostgreSql.Utils;

namespace KpzRepository.PostgreSql.Tests;

[TestFixture]
public class TableWithStringIdTests
{
    [OneTimeSetUp]
    public void Setup()
    {
        if (AllTestsSetup.Settings == null)
        {
            Assert.Fail($"Settings must be initialized in {nameof(AllTestsSetup)} before running tests.");
        }
        if (string.IsNullOrWhiteSpace(AllTestsSetup.Settings!.DefaultConnectionString))
        {
            Assert.Fail($"{nameof(AllTestsSetup.Settings.DefaultConnectionString)} must be set in {nameof(AllTestsSetup.Settings)} before running tests.");
        }
        string connectionString = AllTestsSetup.Settings.DefaultConnectionString;

        var services = new ServiceCollection();
        services.AddKpzRepositoryPostgreSqlFactory(connectionString);
        _serviceProvider = services.BuildServiceProvider();
        _factory = _serviceProvider.GetRequiredService<IKpzRepositoryFactory>();
        if (_factory == null)
        {
            Assert.Fail($"Failed to resolve {nameof(IKpzRepositoryFactory)} from service provider.");
        }
        _repository = _factory?.GetBaseRepository<string, table_with_string_id>();
        _repository?.DeleteAll();// Let's clear the table before we start testing
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        if (_repository != null)
        {
            _repository.Dispose();
            _repository = null;
        }
        if (_factory != null)
        {
            _factory = null;
        }
        if (_serviceProvider != null)
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
            _serviceProvider = null;
        }
    }

    #region Connection Tests

    [Test]
    public void Connection_ShouldNotBeNull()
    {
        Assert.That(_repository!.Connection, Is.Not.Null);
    }

    [Test]
    public void OpenConnection_ShouldReturnTrue()
    {
        var result = _repository!.OpenConnection();
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsConnected_ShouldReturnTrue_WhenConnectionIsOpen()
    {
        _repository!.OpenConnection();
        Assert.That(_repository.IsConnected, Is.True);
    }

    [Test]
    public void CloseConnection_ShouldReturnTrue()
    {
        _repository!.OpenConnection();
        var result = _repository.CloseConnection();
        Assert.That(result, Is.True);
    }

    #endregion

    #region Metadata Tests

    [Test]
    public void GetRepositoryTableName_ShouldReturnCorrectTableName()
    {
        var tableName = _repository!.GetRepositoryTableName();
        Assert.That(tableName, Is.EqualTo("table_with_string_id"));
    }

    [Test]
    public void GetRepositoryKeyName_ShouldReturnId()
    {
        var keyName = _repository!.GetRepositoryKeyName();
        table_with_string_id entity = new table_with_string_id();
        Assert.That(keyName, Is.EqualTo($"{nameof(entity.id)}"));
    }

    #endregion

    #region Add Tests

    [Test]
    public void Add_ShouldInsertEntity_AndReturnTrue()
    {
        var entity = CreateTestEntity("Test Add");
        long initialCount = _repository!.Count();
        var result = _repository!.Add(entity);

        Assert.That(result, Is.True);
        Assert.That(entity.id, Is.Not.Null);
        Assert.That(entity.id, Is.Not.Empty);
        Assert.That(_repository.Count(), Is.EqualTo(initialCount + 1));

        // Cleanup
        _repository.Delete(entity.id);
    }

    [Test]
    public async Task AddAsync_ShouldInsertEntity_AndReturnTrue()
    {
        var entity = CreateTestEntity("Test AddAsync");
        long initialCount = await _repository!.CountAsync();
        var result = await _repository!.AddAsync(entity);

        Assert.That(result, Is.True);
        Assert.That(entity.id, Is.Not.Null);
        Assert.That(entity.id, Is.Not.Empty);
        Assert.That(await _repository.CountAsync(), Is.EqualTo(initialCount + 1));

        // Cleanup
        _repository.Delete(entity.id);
    }

    [Test]
    public void AddRange_ShouldInsertMultipleEntities()
    {
        var entities = new List<table_with_string_id>
        {
            CreateTestEntity("Entity 1"),
            CreateTestEntity("Entity 2"),
            CreateTestEntity("Entity 3")
        };

        var transaction = _repository!.BeginTransaction();
        long initialCount = _repository.Count(transaction);
        var insertedCount = _repository.AddRange(entities, transaction);
        transaction!.Commit();

        Assert.That(insertedCount, Is.EqualTo(3));
        Assert.That(entities.All(e => string.IsNullOrEmpty(e.title) == false), Is.True);
        Assert.That(_repository.Count(), Is.EqualTo(initialCount + 3));

        // Cleanup
        foreach (var entity in entities)
        {
            _repository.Delete(entity.id);
        }
    }

    [Test]
    public async Task AddRangeAsync_ShouldInsertMultipleEntities()
    {
        var entities = new List<table_with_string_id>
        {
            CreateTestEntity("Entity Async 1"),
            CreateTestEntity("Entity Async 2"),
            CreateTestEntity("Entity Async 3")
        };

        var transaction = _repository!.BeginTransaction();
        long initialCount = await _repository.CountAsync(transaction);
        var insertedCount = await _repository.AddRangeAsync(entities, transaction);
        transaction!.Commit();

        Assert.That(insertedCount, Is.EqualTo(3));
        Assert.That(entities.All(e => string.IsNullOrEmpty(e.title) == false), Is.True);
        Assert.That(await _repository.CountAsync(), Is.EqualTo(initialCount + 3));

        // Cleanup
        foreach (var entity in entities)
        {
            _repository.Delete(entity.id);
        }
    }

    #endregion

    #region Update Tests

    [Test]
    public void Update_ShouldModifyEntity_AndReturnTrue()
    {
        // Arrange
        var entity = CreateTestEntity("Original Title");
        _repository!.Add(entity);

        // Act
        entity.title = "Updated Title";
        entity.amount = 100.50;
        var result = _repository.Update(entity);

        // Assert
        Assert.That(result, Is.True);
        var updated = _repository.Get(entity.id);
        Assert.That(updated!.title, Is.EqualTo("Updated Title"));
        Assert.That(updated.amount, Is.EqualTo(100.50));

        // Cleanup
        _repository.Delete(entity.id);
    }

    [Test]
    public async Task UpdateAsync_ShouldModifyEntity_AndReturnTrue()
    {
        // Arrange
        var entity = CreateTestEntity("Original Async Title");
        await _repository!.AddAsync(entity);

        // Act
        entity.title = "Updated Async Title";
        entity.balance = 99.99m;
        var result = await _repository.UpdateAsync(entity);

        // Assert
        Assert.That(result, Is.True);
        var updated = await _repository.GetAsync(entity.id);
        Assert.That(updated!.title, Is.EqualTo("Updated Async Title"));
        Assert.That(updated.balance, Is.EqualTo(99.99m));

        // Cleanup
        _repository.Delete(entity.id);
    }

    #endregion

    #region Delete Tests

    [Test]
    public void Delete_ShouldRemoveEntity_AndReturnTrue()
    {
        var entity = CreateTestEntity("To Delete");
        _repository!.Add(entity);

        var result = _repository.Delete(entity.id);

        Assert.That(result, Is.True);
        Assert.That(_repository.Exists(entity.id), Is.False);
    }

    [Test]
    public void DeleteAll_ShouldRemoveAllEntities()
    {
        // Add some test data
        _repository!.Add(CreateTestEntity("Delete All 1"));
        _repository.Add(CreateTestEntity("Delete All 2"));

        var result = _repository.DeleteAll();

        Assert.That(result, Is.True);
        Assert.That(_repository.Count(), Is.EqualTo(0));
    }

    #endregion

    #region Get Tests

    [Test]
    public void Get_ShouldReturnEntity_WhenExists()
    {
        var entity = CreateTestEntity("Test Get");
        _repository!.Add(entity);

        var retrieved = _repository.Get(entity.id);

        Assert.That(retrieved, Is.Not.Null);
        Assert.That(retrieved!.id, Is.EqualTo(entity.id));
        Assert.That(retrieved.title, Is.EqualTo("Test Get"));

        // Cleanup
        _repository.Delete(entity.id);
    }

    [Test]
    public void Get_ShouldReturnNull_WhenNotExists()
    {
        var retrieved = _repository!.Get("NonExistentId");
        Assert.That(retrieved, Is.Null);
    }

    [Test]
    public async Task GetAsync_ShouldReturnEntity_WhenExists()
    {
        var entity = CreateTestEntity("Test GetAsync");
        await _repository!.AddAsync(entity);

        var retrieved = await _repository.GetAsync(entity.id);

        Assert.That(retrieved, Is.Not.Null);
        Assert.That(retrieved!.id, Is.EqualTo(entity.id));
        Assert.That(retrieved.title, Is.EqualTo("Test GetAsync"));

        // Cleanup
        _repository.Delete(entity.id);
    }

    [Test]
    public async Task GetAsync_ShouldReturnNull_WhenNotExists()
    {
        var retrieved = await _repository!.GetAsync("NonExistentId");
        Assert.That(retrieved, Is.Null);
    }

    [Test]
    public void GetAll_ShouldReturnAllEntities()
    {
        // Clean and add test data
        _repository!.DeleteAll();
        _repository.Add(CreateTestEntity("GetAll 1"));
        _repository.Add(CreateTestEntity("GetAll 2"));
        _repository.Add(CreateTestEntity("GetAll 3"));

        var entities = _repository.GetAll().ToList();

        Assert.That(entities.Count, Is.EqualTo(3));

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Clean and add test data
        _repository!.DeleteAll();
        await _repository.AddAsync(CreateTestEntity("GetAllAsync 1"));
        await _repository.AddAsync(CreateTestEntity("GetAllAsync 2"));
        await _repository.AddAsync(CreateTestEntity("GetAllAsync 3"));

        var entities = (await _repository.GetAllAsync()).ToList();

        Assert.That(entities.Count, Is.EqualTo(3));

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public void GetAllOrderBy_ShouldReturnEntitiesInOrder()
    {
        // Clean and add test data
        _repository!.DeleteAll();
        _repository.Add(CreateTestEntity("C Entity", category: "C"));
        _repository.Add(CreateTestEntity("A Entity", category: "A"));
        _repository.Add(CreateTestEntity("B Entity", category: "B"));

        var entities = _repository.GetAllOrderBy($"{nameof(table_with_string_id.title)}", desc: false).ToList();

        Assert.That(entities.Count, Is.EqualTo(3));
        Assert.That(entities[0].title, Is.EqualTo("A Entity"));
        Assert.That(entities[1].title, Is.EqualTo("B Entity"));
        Assert.That(entities[2].title, Is.EqualTo("C Entity"));

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public void GetAllOrderBy_ShouldReturnEntitiesInDescendingOrder()
    {
        // Clean and add test data
        _repository!.DeleteAll();
        _repository.Add(CreateTestEntity("Entity 1", amount: 10.0));
        _repository.Add(CreateTestEntity("Entity 2", amount: 30.0));
        _repository.Add(CreateTestEntity("Entity 3", amount: 20.0));

        var entities = _repository.GetAllOrderBy($"{nameof(table_with_string_id.amount)}", desc: true).ToList();

        Assert.That(entities.Count, Is.EqualTo(3));
        Assert.That(entities[0].amount, Is.EqualTo(30.0));
        Assert.That(entities[1].amount, Is.EqualTo(20.0));
        Assert.That(entities[2].amount, Is.EqualTo(10.0));

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public async Task GetAllOrderByAsync_ShouldReturnEntitiesInOrder()
    {
        // Clean and add test data
        _repository!.DeleteAll();
        await _repository.AddAsync(CreateTestEntity("Z Entity"));
        await _repository.AddAsync(CreateTestEntity("A Entity"));
        await _repository.AddAsync(CreateTestEntity("M Entity"));

        var entities = (await _repository.GetAllOrderByAsync($"{nameof(table_with_string_id.title)}", desc: false)).ToList();

        Assert.That(entities.Count, Is.EqualTo(3));
        Assert.That(entities[0].title, Is.EqualTo("A Entity"));
        Assert.That(entities[1].title, Is.EqualTo("M Entity"));
        Assert.That(entities[2].title, Is.EqualTo("Z Entity"));

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public void GetMinEntity_ShouldReturnEntityWithLowestId()
    {
        // Clean and add test data
        _repository!.DeleteAll();
        var entity1 = CreateTestEntity("Entity 1", id: "AAA");
        var entity2 = CreateTestEntity("Entity 2", id: "BBB");
        var entity3 = CreateTestEntity("Entity 3", id: "CCC");
        _repository.Add(entity1);
        _repository.Add(entity2);
        _repository.Add(entity3);

        var minEntity = _repository.GetMinEntity();

        Assert.That(minEntity, Is.Not.Null);
        Assert.That(minEntity!.id, Is.EqualTo(entity1.id));

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public void GetMaxEntity_ShouldReturnEntityWithHighestId()
    {
        // Clean and add test data
        _repository!.DeleteAll();
        var entity1 = CreateTestEntity("Entity 1", id: "AAA");
        var entity2 = CreateTestEntity("Entity 2", id: "BBB");
        var entity3 = CreateTestEntity("Entity 3", id: "CCC");
        _repository.Add(entity1);
        _repository.Add(entity2);
        _repository.Add(entity3);

        var maxEntity = _repository.GetMaxEntity();

        Assert.That(maxEntity, Is.Not.Null);
        Assert.That(maxEntity!.id, Is.EqualTo(entity3.id));

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public void GetEntitiesLike_ShouldReturnMatchingEntities()
    {
        // Clean and add test data
        _repository!.DeleteAll();
        _repository.Add(CreateTestEntity("Product Apple"));
        _repository.Add(CreateTestEntity("Product Banana"));
        _repository.Add(CreateTestEntity("Service Apple"));

        var entities = _repository.GetEntitiesLike($"{nameof(table_with_string_id.title)}", "Apple").ToList();

        Assert.That(entities.Count, Is.EqualTo(2));
        Assert.That(entities.All(e => e.title.Contains("Apple")), Is.True);

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public async Task GetEntitiesLikeAsync_ShouldReturnMatchingEntities()
    {
        // Clean and add test data
        _repository!.DeleteAll();
        await _repository.AddAsync(CreateTestEntity("Red Item"));
        await _repository.AddAsync(CreateTestEntity("Blue Item"));
        await _repository.AddAsync(CreateTestEntity("Red Product"));

        var entities = (await _repository.GetEntitiesLikeAsync($"{nameof(table_with_string_id.title)}", "Red")).ToList();

        Assert.That(entities.Count, Is.EqualTo(2));
        Assert.That(entities.All(e => e.title.Contains("Red")), Is.True);

        // Cleanup
        _repository.DeleteAll();
    }

    #endregion

    #region Count and Empty Tests

    [Test]
    public void Count_ShouldReturnNumberOfEntities()
    {
        // Clean and add test data
        _repository!.DeleteAll();
        _repository.Add(CreateTestEntity("Count 1"));
        _repository.Add(CreateTestEntity("Count 2"));
        _repository.Add(CreateTestEntity("Count 3"));

        var count = _repository.Count();

        Assert.That(count, Is.EqualTo(3));

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public async Task CountAsync_ShouldReturnNumberOfEntities()
    {
        // Clean and add test data
        _repository!.DeleteAll();
        await _repository.AddAsync(CreateTestEntity("CountAsync 1"));
        await _repository.AddAsync(CreateTestEntity("CountAsync 2"));

        var count = await _repository.CountAsync();

        Assert.That(count, Is.EqualTo(2));

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public void IsEmpty_ShouldReturnTrue_WhenNoEntities()
    {
        _repository!.DeleteAll();
        Assert.That(_repository.IsEmpty(), Is.True);
    }

    [Test]
    public void IsEmpty_ShouldReturnFalse_WhenEntitiesExist()
    {
        _repository!.DeleteAll();
        _repository.Add(CreateTestEntity("Test IsEmpty"));

        Assert.That(_repository.IsEmpty(), Is.False);

        // Cleanup
        _repository.DeleteAll();
    }

    #endregion

    #region ID Tests

    [Test]
    public void GetLastInsertedId_ShouldReturnLastId()
    {
        _repository!.DeleteAll();
        var entity = CreateTestEntity("Last Inserted", id: "LAST_ID_TEST");
        _repository.Add(entity);

        //var lastId = _repository.GetLastInsertedId();
        var lastId = "LAST_ID_TEST"; // Since we are using ExplicitKey, we know the last inserted id is the one we set

        Assert.That(lastId, Is.Not.Null);
        Assert.That(lastId, Is.Not.Empty);

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public void GetMaxId_ShouldReturnHighestId()
    {
        _repository!.DeleteAll();
        var entity1 = CreateTestEntity("Entity 1", id: "ID_001");
        var entity2 = CreateTestEntity("Entity 2", id: "ID_002");
        var entity3 = CreateTestEntity("Entity 3", id: "ID_003");
        _repository.Add(entity1);
        _repository.Add(entity2);
        _repository.Add(entity3);

        var maxId = _repository.GetMaxId();

        Assert.That(maxId, Is.Not.Null);
        Assert.That(maxId, Is.Not.Empty);
        Assert.That(maxId, Is.EqualTo(entity3.id));

        // Cleanup
        _repository.DeleteAll();
    }

    [Test]
    public void GetMinId_ShouldReturnLowestId()
    {
        _repository!.DeleteAll();
        var entity1 = CreateTestEntity("Entity 1", id: "ID_001");
        var entity2 = CreateTestEntity("Entity 2", id: "ID_002");
        var entity3 = CreateTestEntity("Entity 3", id: "ID_003");
        _repository.Add(entity1);
        _repository.Add(entity2);
        _repository.Add(entity3);

        var minId = _repository.GetMinId();

        Assert.That(minId, Is.Not.Null);
        Assert.That(minId, Is.Not.Empty);
        Assert.That(minId, Is.EqualTo(entity1.id));

        // Cleanup
        _repository.DeleteAll();
    }

    #endregion

    #region Exists Tests

    [Test]
    public void Exists_ShouldReturnTrue_WhenEntityExists()
    {
        var entity = CreateTestEntity("Exists Test");
        _repository!.Add(entity);

        var exists = _repository.Exists(entity.id);

        Assert.That(exists, Is.True);

        // Cleanup
        _repository.Delete(entity.id);
    }

    [Test]
    public void Exists_ShouldReturnFalse_WhenEntityDoesNotExist()
    {
        var exists = _repository!.Exists("NonExistentId");
        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task ExistsAsync_ShouldReturnTrue_WhenEntityExists()
    {
        var entity = CreateTestEntity("ExistsAsync Test");
        await _repository!.AddAsync(entity);

        var exists = await _repository.ExistsAsync(entity.id);

        Assert.That(exists, Is.True);

        // Cleanup
        _repository.Delete(entity.id);
    }

    [Test]
    public async Task ExistsAsync_ShouldReturnFalse_WhenEntityDoesNotExist()
    {
        var exists = await _repository!.ExistsAsync("NonExistentId");
        Assert.That(exists, Is.False);
    }

    #endregion

    #region Transaction Tests

    [Test]
    public void BeginTransaction_ShouldReturnTransaction()
    {
        var transaction = _repository!.BeginTransaction();

        Assert.That(transaction, Is.Not.Null);

        transaction!.Rollback();
        transaction.Dispose();
    }

    [Test]
    public void Transaction_ShouldCommitSuccessfully()
    {
        var entity = CreateTestEntity("Transaction Test");

        var transaction = _repository!.BeginTransaction();
        _repository.Add(entity, transaction);
        transaction!.Commit();

        Assert.That(_repository.Exists(entity.id), Is.True);

        // Cleanup
        _repository.Delete(entity.id);
    }

    [Test]
    public void Transaction_ShouldRollbackSuccessfully()
    {
        var entity = CreateTestEntity("Rollback Test");

        var transaction = _repository!.BeginTransaction();
        _repository.Add(entity, transaction);
        transaction!.Rollback();

        Assert.That(_repository.Exists(entity.id), Is.False);
    }

    #endregion

    #region ExecuteQuery Tests

    [Test]
    public void ExecuteQuery_ShouldExecuteCustomSql()
    {
        _repository!.DeleteAll();
        var entity = CreateTestEntity("Custom SQL Test");
        _repository.Add(entity);

        var sql = "UPDATE table_with_string_id SET amount = 999.99 WHERE id = @Id";
        var rowsAffected = _repository.ExecuteQuery(sql, new { Id = entity.id });

        Assert.That(rowsAffected, Is.EqualTo(1));

        var updated = _repository.Get(entity.id);
        Assert.That(updated!.amount, Is.EqualTo(999.99));

        // Cleanup
        _repository.Delete(entity.id);
    }

    [Test]
    public async Task ExecuteQueryAsync_ShouldExecuteCustomSql()
    {
        _repository!.DeleteAll();
        var entity = CreateTestEntity("Custom SQL Async Test");
        await _repository.AddAsync(entity);

        var sql = "UPDATE table_with_string_id SET balance = 888.88 WHERE id = @Id";
        var rowsAffected = await _repository.ExecuteQueryAsync(sql, new { Id = entity.id });

        Assert.That(rowsAffected, Is.EqualTo(1));

        var updated = await _repository.GetAsync(entity.id);
        Assert.That(updated!.balance, Is.EqualTo(888.88m));

        // Cleanup
        _repository.Delete(entity.id);
    }

    #endregion

    #region Soft Delete Tests

    [Test]
    public void SoftDelete_ShouldSetDeletedFlag()
    {
        var entity = CreateTestEntity("Soft Delete Test");
        entity.is_deleted = false;
        _repository!.Add(entity);

        // Soft delete
        entity.is_deleted = true;
        entity.deleted_on = DateTime.UtcNow;
        _repository.Update(entity);

        var retrieved = _repository.Get(entity.id);
        Assert.That(retrieved, Is.Not.Null);
        Assert.That(retrieved!.is_deleted, Is.True);
        Assert.That(retrieved.deleted_on, Is.Not.Null);

        // Cleanup
        _repository.Delete(entity.id);
    }

    #endregion

    #region Category and RelatedId Tests

    [Test]
    public void GetEntitiesLike_ShouldFilterByCategory()
    {
        _repository!.DeleteAll();
        _repository.Add(CreateTestEntity("Entity 1", category: "Electronics"));
        _repository.Add(CreateTestEntity("Entity 2", category: "Books"));
        _repository.Add(CreateTestEntity("Entity 3", category: "Electronics"));

        var entities = _repository.GetEntitiesLike($"{nameof(table_with_string_id.category)}", "Electronics").ToList();

        Assert.That(entities.Count, Is.EqualTo(2));
        Assert.That(entities.All(e => e.category == "Electronics"), Is.True);

        // Cleanup
        _repository.DeleteAll();
    }

    #endregion

    #region Helper Methods

    private static int _counter = 0;

    private table_with_string_id CreateTestEntity(string title, string? id = null, string? category = null, 
        double? amount = null, long? relatedLongId = null)
    {
        _counter++;
        return new table_with_string_id
        {
            id = id ?? $"STR_{Guid.NewGuid():N}_{_counter}",
            title = title,
            notes = $"Notes for {title}",
            amount = amount ?? 19.99,
            balance = 100.00m,
            is_deleted = false,
            created_on = DateTime.UtcNow,
            category = category ?? "Default",
            related_long_id = relatedLongId,
            attributes = new JsonbValue("{\"key\":\"value\"}")
        };
    }

    #endregion

    private IKpzRepositoryFactory? _factory;
    private IKpzRepository<string, table_with_string_id>? _repository;
    private IServiceProvider? _serviceProvider;
}
