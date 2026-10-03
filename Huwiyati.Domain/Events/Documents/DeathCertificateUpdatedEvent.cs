namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

public class DeathCertificateUpdatedEvent : BaseEvent
{
    public Guid RelativePersonId { get; set; }
    public string DeceasedName { get; set; } = string.Empty;

    public DeathCertificateUpdatedEvent(Guid relativePersonId, string deceasedName)
    {
        RelativePersonId = relativePersonId;
        DeceasedName = deceasedName;
    }
}
