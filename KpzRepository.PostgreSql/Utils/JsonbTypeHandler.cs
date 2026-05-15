using Dapper;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace KpzRepository.PostgreSql.Utils;

public class JsonbTypeHandler : SqlMapper.TypeHandler<JsonbValue?>
{
    public override void SetValue(IDbDataParameter parameter, JsonbValue? value)
    {
        if(parameter is NpgsqlParameter npgsqlParam)
        {
            npgsqlParam.NpgsqlDbType = NpgsqlDbType.Jsonb;
            npgsqlParam.Value = value?.Value ?? (object)DBNull.Value;
        }
    }

    public override JsonbValue? Parse(object value)
    {
        return new JsonbValue(value.ToString());
    }
}