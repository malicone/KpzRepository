using Dapper.Contrib.Extensions;
using KpzRepository.Model;

namespace KpzRepository.SqlServer.Tests.Model;

[Table("TableWithLongId")]
public class TableWithLongId : BaseEntity<long>
{
    [Key]
    public long Id { get; set; }
    // Basic fields
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    // Numeric
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    // Boolean
    public bool IsActive { get; set; }

    // Date/time
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // GUID
    public Guid ExternalId { get; set; }

    // JSON / flexible
    public string? Metadata { get; set; }

    // RowVersion (timestamp)
    [Write(false)]
    public byte[] RowVersion { get; set; } = null!;
}