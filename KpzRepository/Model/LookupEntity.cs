using Dapper.Contrib.Extensions;

namespace KpzRepository.Model;

/// <summary>
/// As a rule, lookup entities are entities that are used to store data that have few records and is 
/// not changed frequently. Usually, these entities are used in dropdown lists like country, category, currency etc.
/// </summary>
/// <remarks>Lowercase snake_case is generally preferred for cross-platform compatibility. So we use it for table and column names.</remarks>
/// <typeparam name="TKey">Key (Id) type. It can be int, string, long etc.</typeparam>
public abstract class LookupEntity<TKey> : BaseEntity<TKey>
{
    public string? name { get; set; }
    public string? code { get; set; }
    public string? description { get; set; }
    public long? display_order { get; set; }
    public bool? is_active { get; set; }

    [Write(false)]
    public bool is_active_value => is_active ?? false;

    public override string GetDefaultSortFieldName()
    {
        return nameof(display_order);
    }
}
