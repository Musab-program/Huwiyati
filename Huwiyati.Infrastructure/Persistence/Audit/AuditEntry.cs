namespace Huwiyati.Infrastructure.Persistence.Audit;

using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Huwiyati.Domain.Entities.Audit;

/// <summary>
/// Helper class for tracking entity changes and constructing <see cref="AuditLog"/> records during EF Core change tracking.
/// </summary>
public class AuditEntry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuditEntry"/> class with the tracked entity entry.
    /// </summary>
    /// <param name="entry">The EF Core tracked entity entry.</param>
    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }

    /// <summary>
    /// Gets the underlying EF Core entity entry being tracked.
    /// </summary>
    public EntityEntry Entry { get; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who performed the action.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the action type performed on the entity (e.g., Created, Updated, Deleted).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the entity being modified.
    /// </summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the primary key identifier of the modified entity.
    /// </summary>
    public string EntityId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC timestamp when the change occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets the dictionary of property names and their original values prior to modification.
    /// </summary>
    public Dictionary<string, object?> OldValues { get; } = new();

    /// <summary>
    /// Gets the dictionary of property names and their updated values.
    /// </summary>
    public Dictionary<string, object?> NewValues { get; } = new();

    /// <summary>
    /// Gets the list of column/property names that were altered during this transaction.
    /// </summary>
    public List<string> ChangedColumns { get; } = new();

    /// <summary>
    /// Converts the tracked audit entry into an <see cref="AuditLog"/> persistence entity.
    /// </summary>
    /// <returns>A populated <see cref="AuditLog"/> instance ready for database persistence.</returns>
    public AuditLog ToAuditLog()
    {
        return new AuditLog
        {
            UserId = UserId,
            Action = Action,
            EntityName = EntityName,
            EntityId = EntityId,
            Timestamp = Timestamp,
            OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
            AffectedColumns = ChangedColumns.Count == 0 ? null : JsonSerializer.Serialize(ChangedColumns)
        };
    }
}
