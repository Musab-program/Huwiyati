namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an official death certificate has been registered and issued.
/// </summary>
public class DeathCertificateIssuedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the deceased person.
    /// </summary>
    public Guid DeceasedPersonId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the deceased citizen.
    /// </summary>
    public string DeceasedName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeathCertificateIssuedEvent"/> class.
    /// </summary>
    /// <param name="deceasedPersonId">The unique identifier of the deceased person.</param>
    /// <param name="deceasedName">The full name of the deceased citizen.</param>
    public DeathCertificateIssuedEvent(Guid deceasedPersonId, string deceasedName)
    {
        DeceasedPersonId = deceasedPersonId;
        DeceasedName = deceasedName;
    }
}
