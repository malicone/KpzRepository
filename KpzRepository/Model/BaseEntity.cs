using System.Reflection;
using Dapper.Contrib.Extensions;
using KpzRepository.Common;

namespace KpzRepository.Model;

/// <summary>
/// Base class for all entities (models) in a database. Every entity is table in a database (every entity maps to a table in a database).
/// </summary>
/// <typeparam name="TKey">Primary key (Id) type. It can be int, string, long etc.</typeparam>
public abstract class BaseEntity<TKey>
{
    /// <summary>
    /// Primary key of the entity/table. Primary key defined with [Key] or [ExplicitKey] attribute.
    /// Hide the Id property in the derived classes if you want to use [ExplicitKey] attribute.
    /// <code>
    /// [ExplicitKey]
    /// public new long Id { get; set; }
    /// </code>
    /// </summary>
    [Key]
    public TKey Id { get; set; } = default!;

    public virtual string GetTableName()
    {
        string? tableName = GetType().GetAttributeValue<TableAttribute, string>(attribute => attribute.Name);
        if (tableName == null)
        {
            return GetType().Name;
        }
        return tableName;
    }
    public virtual string GetKeyName()
    {
        PropertyInfo? keyProperty = GetKeyProperty();
        if (keyProperty != null)
        {
            return keyProperty.Name;
        }
        return string.Empty;
    }

    public virtual string GetDefaultSortFieldName()
    {
        return GetKeyName();
    }

    protected virtual Type? GetFieldType(string fieldName)
    {
        var property = GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(fieldName));
        if (property != null)
        {
            return property.PropertyType;
        }
        return null;
    }
    public virtual bool IsFieldTypeOfString(string fieldName)
    {
        Type? columnType = GetFieldType(fieldName);
        if (columnType != null)
        {
            return columnType == typeof(string);
        }
        return false;
    }

    protected virtual PropertyInfo? GetKeyProperty()
    {
        var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var currentProperty in properties)
        {
            var attributes = currentProperty.GetCustomAttributes(typeof(KeyAttribute), false);
            if (attributes.Length > 0)
            {
                return currentProperty;
            }
            attributes = currentProperty.GetCustomAttributes(typeof(ExplicitKeyAttribute), false);
            if (attributes.Length > 0)
            {
                return currentProperty;
            }
        }
        string tableName = GetTableName();
        string message = $"Attribute [Key] or [ExplicitKey] is not specified in the entity {tableName}";
        throw new KeyNotFoundException(message);
    }

    protected virtual IEnumerable<PropertyInfo> GetFieldsAllWritable()
    {
        var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var currentProperty in properties)
        {
            var attributes = currentProperty.GetCustomAttributes(typeof(WriteAttribute), true);
            if (attributes.Length > 0)
            {
                bool isWrite = ((WriteAttribute)attributes[0]).Write;
                if (isWrite == false)
                {
                    continue;
                }
            }
            yield return currentProperty;
        }
    }
}