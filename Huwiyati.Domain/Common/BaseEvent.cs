namespace Huwiyati.Domain.Common;

// Base class for all domain events in Huwiyati system
public abstract class BaseEvent
{
    // Timestamp when the event occurred
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
