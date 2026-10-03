namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

public class DeathCertificateIssuedEvent : BaseEvent
{
    public Guid DeceasedPersonId { get; set; }
    public string DeceasedName { get; set; } = string.Empty;

    public DeathCertificateIssuedEvent(Guid deceasedPersonId, string deceasedName)
    {
        DeceasedPersonId = deceasedPersonId;
        DeceasedName = deceasedName;
    }
}
