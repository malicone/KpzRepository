using Dapper.Contrib.Extensions;

namespace KpzRepository.Model;

/// <summary>
/// Tracked entities are entities/records that need to be tracked for creation, update, and deletion.
/// Thus tracked entities support soft deletion.
/// </summary>
/// <typeparam name="TKey">Key (Id) type. It can be int, string, long etc.</typeparam>
public abstract class TrackedEntity<TKey> : BaseEntity<TKey>
{
    public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }

    [Write(false)]
    public bool IsDeleted => DeletedAt.HasValue;

    public void SetCreateStamp(string? createdBy = null)
    {
        CreatedAt = DateTimeOffset.Now;
        CreatedBy = createdBy;
    }
    public void SetCreateStampIfEmpty(string? createdBy = null)
    {
        if (CreatedAt.HasValue == false)
        {
            SetCreateStamp(createdBy);
        }
    }
    public void SetUpdateStamp(string? updatedBy = null)
    {
        UpdatedAt = DateTimeOffset.Now;
        UpdatedBy = updatedBy;
    }
    public void SetUpdateStampIfEmpty(string? updatedBy = null)
    {
        if (UpdatedAt.HasValue == false)
        {
            SetUpdateStamp(updatedBy);
        }
    }
    public void SetDeleteStamp(string? deletedBy = null)
    {
        DeletedAt = DateTimeOffset.Now;
        DeletedBy = deletedBy;
    }
    public void SetDeleteStampIfEmpty(string? deletedBy = null)
    {
        if (DeletedAt.HasValue == false)
        {
            SetDeleteStamp(deletedBy);
        }
    }
}