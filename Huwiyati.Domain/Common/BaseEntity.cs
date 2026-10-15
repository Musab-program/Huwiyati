namespace Huwiyati.Domain.Common;

/// <summary>
/// Represents the base entity for all domain entities in the Huwiyati system.
/// Generates sequentially sortable unique identifiers using UUID Version 7 (RFC 9562).
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique primary key identifier for the entity.
    /// Uses sequential Guid Version 7 containing an embedded millisecond timestamp.
    /// </summary>
    public Guid Id { get; set; } = Guid.CreateVersion7();
}
