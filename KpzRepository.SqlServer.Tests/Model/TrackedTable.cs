using Dapper.Contrib.Extensions;
using KpzRepository.Model;

namespace KpzRepository.SqlServer.Tests.Model;

public class TrackedTable : TrackedEntity<long>
{
    [Key]
    public long Id { get; set; }
    // All fields are inherited from TrackedEntity<long>
}
