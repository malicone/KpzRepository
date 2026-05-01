using Dapper.Contrib.Extensions;

namespace KpzRepository.Model;

/// <summary>
/// As a rule, lookup entities are entities that are used to store data that have few records and is 
/// not changed frequently. Usually, these entities are used in dropdown lists like country, category, currency etc.
/// </summary>
/// <remarks>Field names are in PascalCase to match C# naming conventions. At the same time, Dapper is configured to match names in snake_case in the database:
/// Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;</remarks>
/// <typeparam name="TKey">Key (Id) type. It can be int, string, long etc.</typeparam>
public abstract class LookupEntity<TKey> : BaseEntity<TKey>
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public long? DisplayOrder { get; set; }
    public bool? IsActive { get; set; }

    [Write(false)]
    public bool IsActiveValue => IsActive ?? false;

    public override string GetDefaultSortFieldName()
    {
        return nameof(DisplayOrder);
    }
}
