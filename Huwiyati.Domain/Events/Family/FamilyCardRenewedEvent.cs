namespace Huwiyati.Domain.Events.Family;

using Huwiyati.Domain.Common;

public class FamilyCardRenewedEvent : BaseEvent
{
    public Guid HeadOfFamilyPersonId { get; set; }
    public string FamilyNumber { get; set; } = string.Empty;

    public FamilyCardRenewedEvent(Guid headOfFamilyPersonId, string familyNumber)
    {
        HeadOfFamilyPersonId = headOfFamilyPersonId;
        FamilyNumber = familyNumber;
    }
}
