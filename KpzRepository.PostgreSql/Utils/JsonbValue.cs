namespace KpzRepository.PostgreSql.Utils;

public class JsonbValue
{
    public string? Value { get; set; }
    public JsonbValue(string? value) => Value = value;
}