using Dapper.Contrib.Extensions;

namespace KpzRepository.Model;

/// <summary>
/// Tracked entities are entities/records that need to be tracked for creation, update, and deletion.
/// Thus tracked entities support soft deletion.
/// </summary>
/// <remarks>Lowercase snake_case is generally preferred for cross-platform compatibility. So we use it for table and column names.</remarks>
/// <typeparam name="TKey">Key (Id) type. It can be int, string, long etc.</typeparam>
public abstract class TrackedEntity<TKey> : BaseEntity<TKey>
{
    public DateTimeOffset? created_at { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? updated_at { get; set; }
    public DateTimeOffset? deleted_at { get; set; }

    public string? created_by { get; set; }
    public string? updated_by { get; set; }
    public string? deleted_by { get; set; }

    [Write(false)]
    public bool is_deleted => deleted_at.HasValue;

    public void SetCreateStamp(string? createdBy = null)
    {
        created_at = DateTimeOffset.Now;
        created_by = createdBy;
    }
    public void SetCreateStampOnlyIfEmpty(string? createdBy = null)
    {
        if (created_at.HasValue == false)
        {
            SetCreateStamp(createdBy);
        }
    }
    public void SetUpdateStamp(string? updatedBy = null)
    {
        updated_at = DateTimeOffset.Now;
        updated_by = updatedBy;
    }
    public void SetUpdateStampOnlyIfEmpty(string? updatedBy = null)
    {
        if (updated_at.HasValue == false)
        {
            SetUpdateStamp(updatedBy);
        }
    }
    public void SetDeleteStamp(string? deletedBy = null)
    {
        deleted_at = DateTimeOffset.Now;
        deleted_by = deletedBy;
    }
    public void SetDeleteStampOnlyIfEmpty(string? deletedBy = null)
    {
        if (deleted_at.HasValue == false)
        {
            SetDeleteStamp(deletedBy);
        }
    }
}