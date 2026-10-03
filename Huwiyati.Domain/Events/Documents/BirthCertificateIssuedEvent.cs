namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

public class BirthCertificateIssuedEvent : BaseEvent
{
    public Guid FatherPersonId { get; set; }
    public string ChildName { get; set; } = string.Empty;

    public BirthCertificateIssuedEvent(Guid fatherPersonId, string childName)
    {
        FatherPersonId = fatherPersonId;
        ChildName = childName;
    }
}
