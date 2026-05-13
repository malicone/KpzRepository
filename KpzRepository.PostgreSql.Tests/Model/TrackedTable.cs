using Dapper.Contrib.Extensions;
using KpzRepository.Model;

namespace KpzRepository.PostgreSql.Tests.Model;

[Table("tracked_table")]
public class TrackedTable : TrackedEntity<long>
{
    [Key]
    public long Id { get; set; }
}
