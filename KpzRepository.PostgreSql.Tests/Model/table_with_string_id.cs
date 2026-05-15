using Dapper.Contrib.Extensions;
using KpzRepository.Model;
using KpzRepository.PostgreSql.Utils;

namespace KpzRepository.PostgreSql.Tests.Model;

[Table("table_with_string_id")]
public class table_with_string_id : BaseEntity<string>
{
    [ExplicitKey]
    public string id { get; set; } = Guid.NewGuid().ToString("N");

    // Basic fields
    public string title { get; set; } = null!;
    public string? notes { get; set; }

    // Numeric
    public double? amount { get; set; }
    public decimal? balance { get; set; }

    // Boolean
    public bool is_deleted { get; set; }

    // Date/time
    public DateTime created_on { get; set; }
    public DateTime? deleted_on { get; set; }

    // Reference-like
    public long? related_long_id { get; set; }

    // JSON / flexible
    public JsonbValue? attributes { get; set; }

    // Indexed field
    public string? category { get; set; }
}