namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

public class ChildDataUpdatedEvent : BaseEvent
{
    public Guid FatherPersonId { get; set; }
    public string ChildName { get; set; } = string.Empty;

    public ChildDataUpdatedEvent(Guid fatherPersonId, string childName)
    {
        FatherPersonId = fatherPersonId;
        ChildName = childName;
    }
}
