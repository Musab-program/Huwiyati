namespace Huwiyati.Domain.Entities.Audit;

using Huwiyati.Domain.Common;

/// <summary>
/// Represents an immutable audit log entry recording detailed historical changes to domain entities.
/// Captures previous and current property values to provide a comprehensive audit trail for sensitive civil records.
/// </summary>
public class AuditLog : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the user or system actor who initiated the change.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the database mutation action performed (e.g., Create, Update, Delete).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the entity/table that underwent modification.
    /// </summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the primary key string representation of the modified entity.
    /// </summary>
    public string EntityId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC timestamp when the audit entry was captured.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the serialized JSON representation of entity property values prior to mutation.
    /// </summary>
    public string? OldValues { get; set; }

    /// <summary>
    /// Gets or sets the serialized JSON representation of entity property values after mutation.
    /// </summary>
    public string? NewValues { get; set; }

    /// <summary>
    /// Gets or sets the serialized list of column/property names that were altered during this operation.
    /// </summary>
    public string? AffectedColumns { get; set; }
}
