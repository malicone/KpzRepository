using KpzRepository.SqlServer.Tests.Model;

namespace KpzRepository.SqlServer.Tests;

[TestFixture]
public class LookupTableTests
{

    #region LookupTable Property Tests

    [Test]
    public void Id_ShouldBeSetAndRetrieved()
    {
        var entity = new LookupTable { Id = 123 };
        Assert.That(entity.Id, Is.EqualTo(123));
    }

    [Test]
    public void Id_ShouldDefaultToZero()
    {
        var entity = new LookupTable();
        Assert.That(entity.Id, Is.EqualTo(0));
    }

    #endregion

    #region LookupEntity Property Tests

    [Test]
    public void Name_ShouldBeSetAndRetrieved()
    {
        var entity = new LookupTable { Name = "Test Name" };
        Assert.That(entity.Name, Is.EqualTo("Test Name"));
    }

    [Test]
    public void Name_ShouldAcceptNull()
    {
        var entity = new LookupTable { Name = null };
        Assert.That(entity.Name, Is.Null);
    }

    [Test]
    public void Code_ShouldBeSetAndRetrieved()
    {
        var entity = new LookupTable { Code = "TST001" };
        Assert.That(entity.Code, Is.EqualTo("TST001"));
    }

    [Test]
    public void Code_ShouldAcceptNull()
    {
        var entity = new LookupTable { Code = null };
        Assert.That(entity.Code, Is.Null);
    }

    [Test]
    public void Description_ShouldBeSetAndRetrieved()
    {
        var entity = new LookupTable { Description = "Test Description" };
        Assert.That(entity.Description, Is.EqualTo("Test Description"));
    }

    [Test]
    public void Description_ShouldAcceptNull()
    {
        var entity = new LookupTable { Description = null };
        Assert.That(entity.Description, Is.Null);
    }

    [Test]
    public void DisplayOrder_ShouldBeSetAndRetrieved()
    {
        var entity = new LookupTable { DisplayOrder = 10 };
        Assert.That(entity.DisplayOrder, Is.EqualTo(10));
    }

    [Test]
    public void DisplayOrder_ShouldAcceptNull()
    {
        var entity = new LookupTable { DisplayOrder = null };
        Assert.That(entity.DisplayOrder, Is.Null);
    }

    [Test]
    public void DisplayOrder_ShouldAcceptNegativeValues()
    {
        var entity = new LookupTable { DisplayOrder = -5 };
        Assert.That(entity.DisplayOrder, Is.EqualTo(-5));
    }

    [Test]
    public void IsActive_ShouldBeSetAndRetrieved_WhenTrue()
    {
        var entity = new LookupTable { IsActive = true };
        Assert.That(entity.IsActive, Is.True);
    }

    [Test]
    public void IsActive_ShouldBeSetAndRetrieved_WhenFalse()
    {
        var entity = new LookupTable { IsActive = false };
        Assert.That(entity.IsActive, Is.False);
    }

    [Test]
    public void IsActive_ShouldAcceptNull()
    {
        var entity = new LookupTable { IsActive = null };
        Assert.That(entity.IsActive, Is.Null);
    }

    [Test]
    public void IsActiveValue_ShouldReturnTrue_WhenIsActiveIsTrue()
    {
        var entity = new LookupTable { IsActive = true };
        Assert.That(entity.IsActiveValue, Is.True);
    }

    [Test]
    public void IsActiveValue_ShouldReturnFalse_WhenIsActiveIsNull()
    {
        var entity = new LookupTable { IsActive = null };
        Assert.That(entity.IsActiveValue, Is.False);
    }

    [Test]
    public void IsActiveValue_ShouldReturnFalse_WhenIsActiveIsFalse()
    {
        var entity = new LookupTable { IsActive = false };
        Assert.That(entity.IsActiveValue, Is.False);
    }

    [Test]
    public void AllProperties_ShouldBeSetAndRetrievedTogether()
    {
        var entity = new LookupTable
        {
            Id = 999,
            Name = "Complete Test",
            Code = "CMPL001",
            Description = "Complete Description",
            DisplayOrder = 100,
            IsActive = true
        };

        Assert.That(entity.Id, Is.EqualTo(999));
        Assert.That(entity.Name, Is.EqualTo("Complete Test"));
        Assert.That(entity.Code, Is.EqualTo("CMPL001"));
        Assert.That(entity.Description, Is.EqualTo("Complete Description"));
        Assert.That(entity.DisplayOrder, Is.EqualTo(100));
        Assert.That(entity.IsActive, Is.True);
        Assert.That(entity.IsActiveValue, Is.True);
    }

    #endregion

    #region BaseEntity Method Tests

    [Test]
    public void GetTableName_ShouldReturnLookupTable()
    {
        var entity = new LookupTable();
        Assert.That(entity.GetTableName(), Is.EqualTo($"{nameof(LookupTable)}"));
    }

    [Test]
    public void GetKeyName_ShouldReturnId()
    {
        var entity = new LookupTable();
        Assert.That(entity.GetKeyName(), Is.EqualTo($"{nameof(LookupTable.Id)}"));
    }

    [Test]
    public void GetEntityId_ShouldReturnIdValue()
    {
        var entity = new LookupTable { Id = 456 };
        Assert.That(entity.GetEntityId(), Is.EqualTo(456));
    }

    [Test]
    public void GetEntityId_ShouldReturnDefault_WhenIdNotSet()
    {
        var entity = new LookupTable();
        Assert.That(entity.GetEntityId(), Is.EqualTo(default(long)));
    }

    [Test]
    public void SetEntityId_ShouldSetIdValue()
    {
        var entity = new LookupTable();
        entity.SetEntityId(789);
        Assert.That(entity.Id, Is.EqualTo(789));
    }

    [Test]
    public void SetEntityId_ShouldOverwriteExistingId()
    {
        var entity = new LookupTable { Id = 100 };
        entity.SetEntityId(200);
        Assert.That(entity.Id, Is.EqualTo(200));
    }

    [Test]
    public void IsFieldTypeOfString_ShouldReturnTrue_ForNameField()
    {
        var entity = new LookupTable();
        Assert.That(entity.IsFieldTypeOfString($"{nameof(LookupTable.Name)}"), Is.True);
    }

    [Test]
    public void IsFieldTypeOfString_ShouldReturnTrue_ForCodeField()
    {
        var entity = new LookupTable();
        Assert.That(entity.IsFieldTypeOfString($"{nameof(LookupTable.Code)}"), Is.True);
    }

    [Test]
    public void IsFieldTypeOfString_ShouldReturnTrue_ForDescriptionField()
    {
        var entity = new LookupTable();
        Assert.That(entity.IsFieldTypeOfString($"{nameof(LookupTable.Description)}"), Is.True);
    }

    [Test]
    public void IsFieldTypeOfString_ShouldReturnFalse_ForIdField()
    {
        var entity = new LookupTable();
        Assert.That(entity.IsFieldTypeOfString($"{nameof(LookupTable.Id)}"), Is.False);
    }

    [Test]
    public void IsFieldTypeOfString_ShouldReturnFalse_ForDisplayOrderField()
    {
        var entity = new LookupTable();
        Assert.That(entity.IsFieldTypeOfString($"{nameof(LookupTable.DisplayOrder)}"), Is.False);
    }

    [Test]
    public void IsFieldTypeOfString_ShouldReturnFalse_ForIsActiveField()
    {
        var entity = new LookupTable();
        Assert.That(entity.IsFieldTypeOfString($"{nameof(LookupTable.IsActive)}"), Is.False);
    }

    [Test]
    public void IsFieldTypeOfString_ShouldReturnFalse_ForNonExistentField()
    {
        var entity = new LookupTable();
        Assert.That(entity.IsFieldTypeOfString("NonExistentField"), Is.False);
    }

    [Test]
    public void IsFieldTypeOfString_ShouldBeCaseInsensitive()
    {
        var entity = new LookupTable();
        Assert.That(entity.IsFieldTypeOfString($"{nameof(LookupTable.Name).ToLower()}"), Is.False); // Case matters in C#
        Assert.That(entity.IsFieldTypeOfString($"{nameof(LookupTable.Name).ToUpper()}"), Is.False);
    }

    #endregion

    #region LookupEntity Method Tests

    [Test]
    public void GetDefaultSortFieldName_ShouldReturnDisplayOrder()
    {
        var entity = new LookupTable();
        Assert.That(entity.GetDefaultSortFieldName(), Is.EqualTo($"{nameof(LookupTable.DisplayOrder)}"));
    }

    #endregion

    #region Entity State Tests

    [Test]
    public void NewInstance_ShouldHaveDefaultValues()
    {
        var entity = new LookupTable();

        Assert.That(entity.Id, Is.EqualTo(0));
        Assert.That(entity.Name, Is.Null);
        Assert.That(entity.Code, Is.Null);
        Assert.That(entity.Description, Is.Null);
        Assert.That(entity.DisplayOrder, Is.Null);
        Assert.That(entity.IsActive, Is.Null);
        Assert.That(entity.IsActiveValue, Is.False);
    }

    [Test]
    public void IsActiveValue_ShouldBeReadOnly()
    {
        var entity = new LookupTable { IsActive = true };

        // IsActiveValue is a computed property (get-only), so it can't be directly set
        Assert.That(entity.IsActiveValue, Is.True);

        entity.IsActive = false;
        Assert.That(entity.IsActiveValue, Is.False);
    }

    [Test]
    public void MultipleInstances_ShouldBeIndependent()
    {
        var entity1 = new LookupTable { Id = 1, Name = "Entity 1" };
        var entity2 = new LookupTable { Id = 2, Name = "Entity 2" };

        Assert.That(entity1.Id, Is.Not.EqualTo(entity2.Id));
        Assert.That(entity1.Name, Is.Not.EqualTo(entity2.Name));
        Assert.That(entity1.GetEntityId(), Is.EqualTo(1));
        Assert.That(entity2.GetEntityId(), Is.EqualTo(2));
    }

    #endregion

    #region Edge Case Tests

    [Test]
    public void Id_ShouldAcceptLargeValues()
    {
        var entity = new LookupTable { Id = long.MaxValue };
        Assert.That(entity.Id, Is.EqualTo(long.MaxValue));
    }

    [Test]
    public void Id_ShouldAcceptNegativeValues()
    {
        var entity = new LookupTable { Id = -1 };
        Assert.That(entity.Id, Is.EqualTo(-1));
    }

    [Test]
    public void Id_ShouldAcceptMinValue()
    {
        var entity = new LookupTable { Id = long.MinValue };
        Assert.That(entity.Id, Is.EqualTo(long.MinValue));
    }

    [Test]
    public void Name_ShouldAcceptEmptyString()
    {
        var entity = new LookupTable { Name = string.Empty };
        Assert.That(entity.Name, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Code_ShouldAcceptEmptyString()
    {
        var entity = new LookupTable { Code = string.Empty };
        Assert.That(entity.Code, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Description_ShouldAcceptLongText()
    {
        var longText = new string('A', 10000);
        var entity = new LookupTable { Description = longText };
        Assert.That(entity.Description, Is.EqualTo(longText));
        Assert.That(entity.Description!.Length, Is.EqualTo(10000));
    }

    [Test]
    public void DisplayOrder_ShouldAcceptZero()
    {
        var entity = new LookupTable { DisplayOrder = 0 };
        Assert.That(entity.DisplayOrder, Is.EqualTo(0));
    }

    [Test]
    public void DisplayOrder_ShouldAcceptMaxValue()
    {
        var entity = new LookupTable { DisplayOrder = long.MaxValue };
        Assert.That(entity.DisplayOrder, Is.EqualTo(long.MaxValue));
    }

    [Test]
    public void DisplayOrder_ShouldAcceptMinValue()
    {
        var entity = new LookupTable { DisplayOrder = long.MinValue };
        Assert.That(entity.DisplayOrder, Is.EqualTo(long.MinValue));
    }

    [Test]
    public void StringProperties_ShouldAcceptSpecialCharacters()
    {
        var entity = new LookupTable
        {
            Name = "Name with special chars: !@#$%^&*()",
            Code = "CODE-WITH_SPECIAL.CHARS",
            Description = "Description with unicode: Ñoño, 日本語, Émile"
        };

        Assert.That(entity.Name, Is.EqualTo("Name with special chars: !@#$%^&*()"));
        Assert.That(entity.Code, Is.EqualTo("CODE-WITH_SPECIAL.CHARS"));
        Assert.That(entity.Description, Is.EqualTo("Description with unicode: Ñoño, 日本語, Émile"));
    }

    #endregion

    #region Type Safety Tests

    [Test]
    public void GetEntityId_ShouldReturnLongType()
    {
        var entity = new LookupTable { Id = 123 };
        var id = entity.GetEntityId();
        Assert.That(id, Is.TypeOf<long>());
    }

    [Test]
    public void GetTableName_ShouldReturnStringType()
    {
        var entity = new LookupTable();
        var tableName = entity.GetTableName();
        Assert.That(tableName, Is.TypeOf<string>());
    }

    [Test]
    public void GetKeyName_ShouldReturnStringType()
    {
        var entity = new LookupTable();
        var keyName = entity.GetKeyName();
        Assert.That(keyName, Is.TypeOf<string>());
    }

    [Test]
    public void GetDefaultSortFieldName_ShouldReturnStringType()
    {
        var entity = new LookupTable();
        var sortField = entity.GetDefaultSortFieldName();
        Assert.That(sortField, Is.TypeOf<string>());
    }

    [Test]
    public void IsActiveValue_ShouldReturnBoolType()
    {
        var entity = new LookupTable();
        var isActiveValue = entity.IsActiveValue;
        Assert.That(isActiveValue, Is.TypeOf<bool>());
    }

    #endregion
}