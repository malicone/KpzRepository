using Dapper.Contrib.Extensions;
using KpzRepository.Model;

namespace KpzRepository.PostgreSql.Tests.Model;

[Table("lookup_table")]
public class LookupTable : LookupEntity<long>
{                                                       
    [Key]
    public long Id { get; set; }
}