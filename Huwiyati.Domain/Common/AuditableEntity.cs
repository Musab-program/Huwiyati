namespace Huwiyati.Domain.Common;

/// <summary>
/// Abstract base entity providing foundational audit tracking properties for creation and modification metadata.
/// Automatically populated by the persistence layer during SaveChanges operations.
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the date and time in UTC when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user or system process that created the entity.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time in UTC when the entity was last modified.
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user or system process that last modified the entity.
    /// </summary>
    public string? LastModifiedBy { get; set; }
}
