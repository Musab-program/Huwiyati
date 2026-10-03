namespace Huwiyati.Domain.Events.Family;

using Huwiyati.Domain.Common;

public class FamilyCardCreatedEvent : BaseEvent
{
    public Guid HeadOfFamilyPersonId { get; set; }
    public string FamilyNumber { get; set; } = string.Empty;

    public FamilyCardCreatedEvent(Guid headOfFamilyPersonId, string familyNumber)
    {
        HeadOfFamilyPersonId = headOfFamilyPersonId;
        FamilyNumber = familyNumber;
    }
}
