using Dapper.Contrib.Extensions;
using KpzRepository.Model;
using KpzRepository.PostgreSql.Utils;

namespace KpzRepository.PostgreSql.Tests.Model;

[Table("table_with_long_id")]
public class table_with_long_id : BaseEntity<long>
{
    [Key]
    public long id { get; set; }

    // Basic fields
    public string name { get; set; } = null!;
    public string? description { get; set; }

    // Numeric
    public int quantity { get; set; }
    public decimal price { get; set; }

    // Boolean
    public bool is_active { get; set; }

    // Date/time
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }

    // GUID
    public Guid external_id { get; set; }

    // JSON / flexible
    public JsonbValue? metadata { get; set; }
}