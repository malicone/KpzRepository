using KpzRepository.PostgreSql.Tests.Model;

namespace KpzRepository.PostgreSql.Tests;

[TestFixture]
public class TrackedTableTests
{
    #region TrackedTable Property Tests

    [Test]
    public void Id_ShouldBeSetAndRetrieved()
    {
        var entity = new TrackedTable { Id = 123 };
        Assert.That(entity.Id, Is.EqualTo(123));
    }

    [Test]
    public void Id_ShouldDefaultToZero()
    {
        var entity = new TrackedTable();
        Assert.That(entity.Id, Is.EqualTo(0));
    }

    #endregion

    #region TrackedEntity Property Tests

    [Test]
    public void CreatedAt_ShouldBeSetAndRetrieved()
    {
        var now = DateTimeOffset.Now;
        var entity = new TrackedTable { CreatedAt = now };
        Assert.That(entity.CreatedAt, Is.EqualTo(now));
    }

    [Test]
    public void CreatedAt_ShouldDefaultToCurrentTime()
    {
        var beforeCreation = DateTimeOffset.Now;
        var entity = new TrackedTable();
        var afterCreation = DateTimeOffset.Now;

        Assert.That(entity.CreatedAt, Is.Not.Null);
        Assert.That(entity.CreatedAt!.Value, Is.GreaterThanOrEqualTo(beforeCreation));
        Assert.That(entity.CreatedAt!.Value, Is.LessThanOrEqualTo(afterCreation));
    }

    [Test]
    public void CreatedAt_ShouldAcceptNull()
    {
        var entity = new TrackedTable { CreatedAt = null };
        Assert.That(entity.CreatedAt, Is.Null);
    }

    [Test]
    public void UpdatedAt_ShouldBeSetAndRetrieved()
    {
        var now = DateTimeOffset.Now;
        var entity = new TrackedTable { UpdatedAt = now };
        Assert.That(entity.UpdatedAt, Is.EqualTo(now));
    }

    [Test]
    public void UpdatedAt_ShouldDefaultToNull()
    {
        var entity = new TrackedTable { UpdatedAt = null };
        Assert.That(entity.UpdatedAt, Is.Null);
    }

    [Test]
    public void DeletedAt_ShouldBeSetAndRetrieved()
    {
        var now = DateTimeOffset.Now;
        var entity = new TrackedTable { DeletedAt = now };
        Assert.That(entity.DeletedAt, Is.EqualTo(now));
    }

    [Test]
    public void DeletedAt_ShouldDefaultToNull()
    {
        var entity = new TrackedTable { DeletedAt = null };
        Assert.That(entity.DeletedAt, Is.Null);
    }

    [Test]
    public void CreatedBy_ShouldBeSetAndRetrieved()
    {
        var entity = new TrackedTable { CreatedBy = "user@example.com" };
        Assert.That(entity.CreatedBy, Is.EqualTo("user@example.com"));
    }

    [Test]
    public void CreatedBy_ShouldAcceptNull()
    {
        var entity = new TrackedTable { CreatedBy = null };
        Assert.That(entity.CreatedBy, Is.Null);
    }

    [Test]
    public void UpdatedBy_ShouldBeSetAndRetrieved()
    {
        var entity = new TrackedTable { UpdatedBy = "admin@example.com" };
        Assert.That(entity.UpdatedBy, Is.EqualTo("admin@example.com"));
    }

    [Test]
    public void UpdatedBy_ShouldAcceptNull()
    {
        var entity = new TrackedTable { UpdatedBy = null };
        Assert.That(entity.UpdatedBy, Is.Null);
    }

    [Test]
    public void DeletedBy_ShouldBeSetAndRetrieved()
    {
        var entity = new TrackedTable { DeletedBy = "system@example.com" };
        Assert.That(entity.DeletedBy, Is.EqualTo("system@example.com"));
    }

    [Test]
    public void DeletedBy_ShouldAcceptNull()
    {
        var entity = new TrackedTable { DeletedBy = null };
        Assert.That(entity.DeletedBy, Is.Null);
    }

    [Test]
    public void IsDeleted_ShouldReturnTrue_WhenDeletedAtHasValue()
    {
        var entity = new TrackedTable { DeletedAt = DateTimeOffset.Now };
        Assert.That(entity.IsDeleted, Is.True);
    }

    [Test]
    public void IsDeleted_ShouldReturnFalse_WhenDeletedAtIsNull()
    {
        var entity = new TrackedTable { DeletedAt = null };
        Assert.That(entity.IsDeleted, Is.False);
    }

    [Test]
    public void IsDeleted_ShouldBeReadOnly()
    {
        var entity = new TrackedTable();

        // IsDeleted is a computed property (get-only), so it can't be directly set
        Assert.That(entity.IsDeleted, Is.False);

        entity.DeletedAt = DateTimeOffset.Now;
        Assert.That(entity.IsDeleted, Is.True);
    }

    [Test]
    public void AllProperties_ShouldBeSetAndRetrievedTogether()
    {
        var createdAt = DateTimeOffset.Now.AddDays(-10);
        var updatedAt = DateTimeOffset.Now.AddDays(-5);
        var deletedAt = DateTimeOffset.Now;

        var entity = new TrackedTable
        {
            Id = 999,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
            CreatedBy = "creator@example.com",
            UpdatedBy = "updater@example.com",
            DeletedBy = "deleter@example.com"
        };

        Assert.That(entity.Id, Is.EqualTo(999));
        Assert.That(entity.CreatedAt, Is.EqualTo(createdAt));
        Assert.That(entity.UpdatedAt, Is.EqualTo(updatedAt));
        Assert.That(entity.DeletedAt, Is.EqualTo(deletedAt));
        Assert.That(entity.CreatedBy, Is.EqualTo("creator@example.com"));
        Assert.That(entity.UpdatedBy, Is.EqualTo("updater@example.com"));
        Assert.That(entity.DeletedBy, Is.EqualTo("deleter@example.com"));
        Assert.That(entity.IsDeleted, Is.True);
    }

    #endregion

    #region BaseEntity Method Tests

    [Test]
    public void GetTableName_ShouldReturnTrackedTable()
    {
        var entity = new TrackedTable();
        Assert.That(entity.GetTableName(), Is.EqualTo("tracked_table"));
    }

    [Test]
    public void GetKeyName_ShouldReturnId()
    {
        var entity = new TrackedTable();
        Assert.That(entity.GetKeyName(), Is.EqualTo($"{nameof(TrackedTable.Id)}"));
    }

    [Test]
    public void GetEntityId_ShouldReturnIdValue()
    {
        var entity = new TrackedTable { Id = 456 };
        Assert.That(entity.GetEntityId(), Is.EqualTo(456));
    }

    [Test]
    public void GetEntityId_ShouldReturnDefault_WhenIdNotSet()
    {
        var entity = new TrackedTable();
        Assert.That(entity.GetEntityId(), Is.EqualTo(default(long)));
    }

    [Test]
    public void SetEntityId_ShouldSetIdValue()
    {
        var entity = new TrackedTable();
        entity.SetEntityId(789);
        Assert.That(entity.Id, Is.EqualTo(789));
    }

    [Test]
    public void SetEntityId_ShouldOverwriteExistingId()
    {
        var entity = new TrackedTable { Id = 100 };
        entity.SetEntityId(200);
        Assert.That(entity.Id, Is.EqualTo(200));
    }

    [Test]
    public void IsFieldTypeOfString_ShouldReturnTrue_ForStringFields()
    {
        var entity = new TrackedTable();
        Assert.That(entity.IsFieldTypeOfString($"{nameof(TrackedTable.CreatedBy)}"), Is.True);
        Assert.That(entity.IsFieldTypeOfString($"{nameof(TrackedTable.UpdatedBy)}"), Is.True);
        Assert.That(entity.IsFieldTypeOfString($"{nameof(TrackedTable.DeletedBy)}"), Is.True);
    }

    [Test]
    public void IsFieldTypeOfString_ShouldReturnFalse_ForNonStringFields()
    {
        var entity = new TrackedTable();
        Assert.That(entity.IsFieldTypeOfString($"{nameof(TrackedTable.Id)}"), Is.False);
        Assert.That(entity.IsFieldTypeOfString($"{nameof(TrackedTable.CreatedAt)}"), Is.False);
        Assert.That(entity.IsFieldTypeOfString($"{nameof(TrackedTable.UpdatedAt)}"), Is.False);
        Assert.That(entity.IsFieldTypeOfString($"{nameof(TrackedTable.DeletedAt)}"), Is.False);
    }

    [Test]
    public void IsFieldTypeOfString_ShouldReturnFalse_ForNonExistentField()
    {
        var entity = new TrackedTable();
        Assert.That(entity.IsFieldTypeOfString("NonExistentField"), Is.False);
    }

    [Test]
    public void GetDefaultSortFieldName_ShouldReturnId()
    {
        var entity = new TrackedTable();
        Assert.That(entity.GetDefaultSortFieldName(), Is.EqualTo($"{nameof(TrackedTable.Id)}"));
    }

    #endregion

    #region TrackedEntity Method Tests - SetCreateStamp

    [Test]
    public void SetCreateStamp_ShouldSetCreatedAtToNow()
    {
        var beforeStamp = DateTimeOffset.Now;
        var entity = new TrackedTable { CreatedAt = null };
        entity.SetCreateStamp();
        var afterStamp = DateTimeOffset.Now;

        Assert.That(entity.CreatedAt, Is.Not.Null);
        Assert.That(entity.CreatedAt!.Value, Is.GreaterThanOrEqualTo(beforeStamp));
        Assert.That(entity.CreatedAt!.Value, Is.LessThanOrEqualTo(afterStamp));
    }

    [Test]
    public void SetCreateStamp_ShouldSetCreatedBy_WhenProvided()
    {
        var entity = new TrackedTable();
        entity.SetCreateStamp("user@example.com");

        Assert.That(entity.CreatedBy, Is.EqualTo("user@example.com"));
    }

    [Test]
    public void SetCreateStamp_ShouldSetCreatedByToNull_WhenNotProvided()
    {
        var entity = new TrackedTable { CreatedBy = "old@example.com" };
        entity.SetCreateStamp();

        Assert.That(entity.CreatedBy, Is.Null);
    }

    [Test]
    public void SetCreateStamp_ShouldOverwriteExistingValues()
    {
        var oldTime = DateTimeOffset.Now.AddDays(-1);
        var entity = new TrackedTable 
        { 
            CreatedAt = oldTime,
            CreatedBy = "old@example.com"
        };

        entity.SetCreateStamp("new@example.com");

        Assert.That(entity.CreatedAt, Is.Not.EqualTo(oldTime));
        Assert.That(entity.CreatedBy, Is.EqualTo("new@example.com"));
    }

    [Test]
    public void SetCreateStampOnlyIfEmpty_ShouldSetValues_WhenCreatedAtIsNull()
    {
        var entity = new TrackedTable { CreatedAt = null };
        entity.SetCreateStampOnlyIfEmpty("user@example.com");

        Assert.That(entity.CreatedAt, Is.Not.Null);
        Assert.That(entity.CreatedBy, Is.EqualTo("user@example.com"));
    }

    [Test]
    public void SetCreateStampOnlyIfEmpty_ShouldNotChangeValues_WhenCreatedAtHasValue()
    {
        var existingTime = DateTimeOffset.Now.AddDays(-1);
        var entity = new TrackedTable 
        { 
            CreatedAt = existingTime,
            CreatedBy = "existing@example.com"
        };

        entity.SetCreateStampOnlyIfEmpty("new@example.com");

        Assert.That(entity.CreatedAt, Is.EqualTo(existingTime));
        Assert.That(entity.CreatedBy, Is.EqualTo("existing@example.com"));
    }

    #endregion

    #region TrackedEntity Method Tests - SetUpdateStamp

    [Test]
    public void SetUpdateStamp_ShouldSetUpdatedAtToNow()
    {
        var beforeStamp = DateTimeOffset.Now;
        var entity = new TrackedTable();
        entity.SetUpdateStamp();
        var afterStamp = DateTimeOffset.Now;

        Assert.That(entity.UpdatedAt, Is.Not.Null);
        Assert.That(entity.UpdatedAt!.Value, Is.GreaterThanOrEqualTo(beforeStamp));
        Assert.That(entity.UpdatedAt!.Value, Is.LessThanOrEqualTo(afterStamp));
    }

    [Test]
    public void SetUpdateStamp_ShouldSetUpdatedBy_WhenProvided()
    {
        var entity = new TrackedTable();
        entity.SetUpdateStamp("updater@example.com");

        Assert.That(entity.UpdatedBy, Is.EqualTo("updater@example.com"));
    }

    [Test]
    public void SetUpdateStamp_ShouldSetUpdatedByToNull_WhenNotProvided()
    {
        var entity = new TrackedTable { UpdatedBy = "old@example.com" };
        entity.SetUpdateStamp();

        Assert.That(entity.UpdatedBy, Is.Null);
    }

    [Test]
    public void SetUpdateStamp_ShouldOverwriteExistingValues()
    {
        var oldTime = DateTimeOffset.Now.AddDays(-1);
        var entity = new TrackedTable 
        { 
            UpdatedAt = oldTime,
            UpdatedBy = "old@example.com"
        };

        entity.SetUpdateStamp("new@example.com");

        Assert.That(entity.UpdatedAt, Is.Not.EqualTo(oldTime));
        Assert.That(entity.UpdatedBy, Is.EqualTo("new@example.com"));
    }

    [Test]
    public void SetUpdateStampOnlyIfEmpty_ShouldSetValues_WhenUpdatedAtIsNull()
    {
        var entity = new TrackedTable { UpdatedAt = null };
        entity.SetUpdateStampOnlyIfEmpty("updater@example.com");

        Assert.That(entity.UpdatedAt, Is.Not.Null);
        Assert.That(entity.UpdatedBy, Is.EqualTo("updater@example.com"));
    }

    [Test]
    public void SetUpdateStampOnlyIfEmpty_ShouldNotChangeValues_WhenUpdatedAtHasValue()
    {
        var existingTime = DateTimeOffset.Now.AddDays(-1);
        var entity = new TrackedTable 
        { 
            UpdatedAt = existingTime,
            UpdatedBy = "existing@example.com"
        };

        entity.SetUpdateStampOnlyIfEmpty("new@example.com");

        Assert.That(entity.UpdatedAt, Is.EqualTo(existingTime));
        Assert.That(entity.UpdatedBy, Is.EqualTo("existing@example.com"));
    }

    #endregion

    #region TrackedEntity Method Tests - SetDeleteStamp

    [Test]
    public void SetDeleteStamp_ShouldSetDeletedAtToNow()
    {
        var beforeStamp = DateTimeOffset.Now;
        var entity = new TrackedTable();
        entity.SetDeleteStamp();
        var afterStamp = DateTimeOffset.Now;

        Assert.That(entity.DeletedAt, Is.Not.Null);
        Assert.That(entity.DeletedAt!.Value, Is.GreaterThanOrEqualTo(beforeStamp));
        Assert.That(entity.DeletedAt!.Value, Is.LessThanOrEqualTo(afterStamp));
    }

    [Test]
    public void SetDeleteStamp_ShouldSetDeletedBy_WhenProvided()
    {
        var entity = new TrackedTable();
        entity.SetDeleteStamp("deleter@example.com");

        Assert.That(entity.DeletedBy, Is.EqualTo("deleter@example.com"));
    }

    [Test]
    public void SetDeleteStamp_ShouldSetDeletedByToNull_WhenNotProvided()
    {
        var entity = new TrackedTable { DeletedBy = "old@example.com" };
        entity.SetDeleteStamp();

        Assert.That(entity.DeletedBy, Is.Null);
    }

    [Test]
    public void SetDeleteStamp_ShouldOverwriteExistingValues()
    {
        var oldTime = DateTimeOffset.Now.AddDays(-1);
        var entity = new TrackedTable 
        { 
            DeletedAt = oldTime,
            DeletedBy = "old@example.com"
        };

        entity.SetDeleteStamp("new@example.com");

        Assert.That(entity.DeletedAt, Is.Not.EqualTo(oldTime));
        Assert.That(entity.DeletedBy, Is.EqualTo("new@example.com"));
    }

    [Test]
    public void SetDeleteStamp_ShouldSetIsDeletedToTrue()
    {
        var entity = new TrackedTable();
        Assert.That(entity.IsDeleted, Is.False);

        entity.SetDeleteStamp();

        Assert.That(entity.IsDeleted, Is.True);
    }

    [Test]
    public void SetDeleteStampOnlyIfEmpty_ShouldSetValues_WhenDeletedAtIsNull()
    {
        var entity = new TrackedTable { DeletedAt = null };
        entity.SetDeleteStampOnlyIfEmpty("deleter@example.com");

        Assert.That(entity.DeletedAt, Is.Not.Null);
        Assert.That(entity.DeletedBy, Is.EqualTo("deleter@example.com"));
        Assert.That(entity.IsDeleted, Is.True);
    }

    [Test]
    public void SetDeleteStampOnlyIfEmpty_ShouldNotChangeValues_WhenDeletedAtHasValue()
    {
        var existingTime = DateTimeOffset.Now.AddDays(-1);
        var entity = new TrackedTable 
        { 
            DeletedAt = existingTime,
            DeletedBy = "existing@example.com"
        };

        entity.SetDeleteStampOnlyIfEmpty("new@example.com");

        Assert.That(entity.DeletedAt, Is.EqualTo(existingTime));
        Assert.That(entity.DeletedBy, Is.EqualTo("existing@example.com"));
    }

    #endregion

    #region Entity State Tests

    [Test]
    public void NewInstance_ShouldHaveDefaultValues()
    {
        var entity = new TrackedTable();

        Assert.That(entity.Id, Is.EqualTo(0));
        Assert.That(entity.CreatedAt, Is.Not.Null); // Defaults to DateTimeOffset.Now
        Assert.That(entity.UpdatedAt, Is.Null);
        Assert.That(entity.DeletedAt, Is.Null);
        Assert.That(entity.CreatedBy, Is.Null);
        Assert.That(entity.UpdatedBy, Is.Null);
        Assert.That(entity.DeletedBy, Is.Null);
        Assert.That(entity.IsDeleted, Is.False);
    }

    [Test]
    public void MultipleInstances_ShouldBeIndependent()
    {
        var entity1 = new TrackedTable 
        { 
            Id = 1, 
            CreatedBy = "user1@example.com" 
        };
        var entity2 = new TrackedTable 
        { 
            Id = 2, 
            CreatedBy = "user2@example.com" 
        };

        Assert.That(entity1.Id, Is.Not.EqualTo(entity2.Id));
        Assert.That(entity1.CreatedBy, Is.Not.EqualTo(entity2.CreatedBy));
        Assert.That(entity1.GetEntityId(), Is.EqualTo(1));
        Assert.That(entity2.GetEntityId(), Is.EqualTo(2));
    }

    #endregion

    #region Workflow Tests

    [Test]
    public void TypicalWorkflow_CreateUpdateDelete()
    {
        // Create
        var entity = new TrackedTable();
        entity.SetCreateStamp("creator@example.com");

        Assert.That(entity.CreatedAt, Is.Not.Null);
        Assert.That(entity.CreatedBy, Is.EqualTo("creator@example.com"));
        Assert.That(entity.UpdatedAt, Is.Null);
        Assert.That(entity.DeletedAt, Is.Null);
        Assert.That(entity.IsDeleted, Is.False);

        // Update
        Thread.Sleep(10); // Small delay to ensure different timestamp
        entity.SetUpdateStamp("updater@example.com");

        Assert.That(entity.UpdatedAt, Is.Not.Null);
        Assert.That(entity.UpdatedBy, Is.EqualTo("updater@example.com"));
        Assert.That(entity.UpdatedAt!.Value, Is.GreaterThan(entity.CreatedAt!.Value));
        Assert.That(entity.IsDeleted, Is.False);

        // Delete
        Thread.Sleep(10); // Small delay to ensure different timestamp
        entity.SetDeleteStamp("deleter@example.com");

        Assert.That(entity.DeletedAt, Is.Not.Null);
        Assert.That(entity.DeletedBy, Is.EqualTo("deleter@example.com"));
        Assert.That(entity.DeletedAt!.Value, Is.GreaterThan(entity.UpdatedAt!.Value));
        Assert.That(entity.IsDeleted, Is.True);
    }

    [Test]
    public void ConditionalStamps_ShouldPreserveFirstValues()
    {
        var entity = new TrackedTable();

        // First stamp
        entity.SetCreateStampOnlyIfEmpty("first@example.com");
        var firstCreatedAt = entity.CreatedAt;

        // Try to set again
        Thread.Sleep(10);
        entity.SetCreateStampOnlyIfEmpty("second@example.com");

        // Should still have first values
        Assert.That(entity.CreatedAt, Is.EqualTo(firstCreatedAt));
        Assert.That(entity.CreatedBy, Is.EqualTo("first@example.com"));
    }

    #endregion

    #region Edge Case Tests

    [Test]
    public void Id_ShouldAcceptLargeValues()
    {
        var entity = new TrackedTable { Id = long.MaxValue };
        Assert.That(entity.Id, Is.EqualTo(long.MaxValue));
    }

    [Test]
    public void Id_ShouldAcceptNegativeValues()
    {
        var entity = new TrackedTable { Id = -1 };
        Assert.That(entity.Id, Is.EqualTo(-1));
    }

    [Test]
    public void Id_ShouldAcceptMinValue()
    {
        var entity = new TrackedTable { Id = long.MinValue };
        Assert.That(entity.Id, Is.EqualTo(long.MinValue));
    }

    [Test]
    public void Timestamps_ShouldAcceptPastDates()
    {
        var pastDate = DateTimeOffset.Now.AddYears(-10);
        var entity = new TrackedTable
        {
            CreatedAt = pastDate,
            UpdatedAt = pastDate,
            DeletedAt = pastDate
        };

        Assert.That(entity.CreatedAt, Is.EqualTo(pastDate));
        Assert.That(entity.UpdatedAt, Is.EqualTo(pastDate));
        Assert.That(entity.DeletedAt, Is.EqualTo(pastDate));
    }

    [Test]
    public void Timestamps_ShouldAcceptFutureDates()
    {
        var futureDate = DateTimeOffset.Now.AddYears(10);
        var entity = new TrackedTable
        {
            CreatedAt = futureDate,
            UpdatedAt = futureDate,
            DeletedAt = futureDate
        };

        Assert.That(entity.CreatedAt, Is.EqualTo(futureDate));
        Assert.That(entity.UpdatedAt, Is.EqualTo(futureDate));
        Assert.That(entity.DeletedAt, Is.EqualTo(futureDate));
    }

    [Test]
    public void UserFields_ShouldAcceptEmptyString()
    {
        var entity = new TrackedTable
        {
            CreatedBy = string.Empty,
            UpdatedBy = string.Empty,
            DeletedBy = string.Empty
        };

        Assert.That(entity.CreatedBy, Is.EqualTo(string.Empty));
        Assert.That(entity.UpdatedBy, Is.EqualTo(string.Empty));
        Assert.That(entity.DeletedBy, Is.EqualTo(string.Empty));
    }

    [Test]
    public void UserFields_ShouldAcceptLongStrings()
    {
        var longEmail = new string('a', 256) + "@example.com";
        var entity = new TrackedTable
        {
            CreatedBy = longEmail,
            UpdatedBy = longEmail,
            DeletedBy = longEmail
        };

        Assert.That(entity.CreatedBy, Is.EqualTo(longEmail));
        Assert.That(entity.UpdatedBy, Is.EqualTo(longEmail));
        Assert.That(entity.DeletedBy, Is.EqualTo(longEmail));
    }

    [Test]
    public void UserFields_ShouldAcceptSpecialCharacters()
    {
        var entity = new TrackedTable
        {
            CreatedBy = "user+tag@example.com",
            UpdatedBy = "admin.user@sub-domain.example.com",
            DeletedBy = "system_bot@example.co.uk"
        };

        Assert.That(entity.CreatedBy, Is.EqualTo("user+tag@example.com"));
        Assert.That(entity.UpdatedBy, Is.EqualTo("admin.user@sub-domain.example.com"));
        Assert.That(entity.DeletedBy, Is.EqualTo("system_bot@example.co.uk"));
    }

    [Test]
    public void Timestamps_ShouldHandleDifferentTimeZones()
    {
        var utcTime = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var estTime = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.FromHours(-5));

        var entity = new TrackedTable
        {
            CreatedAt = utcTime,
            UpdatedAt = estTime
        };

        Assert.That(entity.CreatedAt, Is.EqualTo(utcTime));
        Assert.That(entity.UpdatedAt, Is.EqualTo(estTime));
        Assert.That(entity.CreatedAt!.Value.Offset, Is.EqualTo(TimeSpan.Zero));
        Assert.That(entity.UpdatedAt!.Value.Offset, Is.EqualTo(TimeSpan.FromHours(-5)));
    }

    #endregion

    #region Type Safety Tests

    [Test]
    public void GetEntityId_ShouldReturnLongType()
    {
        var entity = new TrackedTable { Id = 123 };
        var id = entity.GetEntityId();
        Assert.That(id, Is.TypeOf<long>());
    }

    [Test]
    public void GetTableName_ShouldReturnStringType()
    {
        var entity = new TrackedTable();
        var tableName = entity.GetTableName();
        Assert.That(tableName, Is.TypeOf<string>());
    }

    [Test]
    public void GetKeyName_ShouldReturnStringType()
    {
        var entity = new TrackedTable();
        var keyName = entity.GetKeyName();
        Assert.That(keyName, Is.TypeOf<string>());
    }

    [Test]
    public void GetDefaultSortFieldName_ShouldReturnStringType()
    {
        var entity = new TrackedTable();
        var sortField = entity.GetDefaultSortFieldName();
        Assert.That(sortField, Is.TypeOf<string>());
    }

    [Test]
    public void IsDeleted_ShouldReturnBoolType()
    {
        var entity = new TrackedTable();
        var isDeleted = entity.IsDeleted;
        Assert.That(isDeleted, Is.TypeOf<bool>());
    }

    [Test]
    public void Timestamps_ShouldReturnNullableDateTimeOffsetType()
    {
        var entity = new TrackedTable();
        Assert.That(entity.CreatedAt, Is.TypeOf<DateTimeOffset>().Or.Null);
        Assert.That(entity.UpdatedAt, Is.Null.Or.TypeOf<DateTimeOffset>());
        Assert.That(entity.DeletedAt, Is.Null.Or.TypeOf<DateTimeOffset>());
    }

    #endregion
}
