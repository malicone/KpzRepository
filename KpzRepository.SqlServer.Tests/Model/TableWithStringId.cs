using Dapper.Contrib.Extensions;
using KpzRepository.Model;

namespace KpzRepository.SqlServer.Tests.Model;

[Table("TableWithStringId")]
public class TableWithStringId : BaseEntity<string>
{
    [ExplicitKey]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    // Basic fields
    public string Title { get; set; } = null!;
    public string? Notes { get; set; }

    // Numeric
    public double? Amount { get; set; }
    public decimal? Balance { get; set; }

    // Boolean
    public bool IsDeleted { get; set; }

    // Date/time
    public DateTime CreatedOn { get; set; }
    public DateTime? DeletedOn { get; set; }

    // Reference-like
    public long? RelatedLongId { get; set; }

    // JSON / flexible
    public string? Attributes { get; set; }

    // Indexed field
    public string? Category { get; set; }
}