namespace Huwiyati.Domain.Common;

/// <summary>
/// Abstract base class for all domain events across the Huwiyati system.
/// Encapsulates event metadata such as creation timestamp.
/// </summary>
public abstract class BaseEvent
{
    /// <summary>
    /// Gets the UTC timestamp when the domain event occurred.
    /// </summary>
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
