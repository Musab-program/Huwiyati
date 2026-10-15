namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an existing death certificate record has been updated or corrected.
/// </summary>
public class DeathCertificateUpdatedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the reporting relative or notifying person.
    /// </summary>
    public Guid RelativePersonId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the deceased person.
    /// </summary>
    public string DeceasedName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeathCertificateUpdatedEvent"/> class.
    /// </summary>
    /// <param name="relativePersonId">The unique identifier of the notifying relative or person.</param>
    /// <param name="deceasedName">The full name of the deceased person.</param>
    public DeathCertificateUpdatedEvent(Guid relativePersonId, string deceasedName)
    {
        RelativePersonId = relativePersonId;
        DeceasedName = deceasedName;
    }
}
