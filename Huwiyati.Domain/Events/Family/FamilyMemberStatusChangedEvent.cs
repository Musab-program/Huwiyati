namespace Huwiyati.Domain.Events.Family;

using Huwiyati.Domain.Common;

public class FamilyMemberStatusChangedEvent : BaseEvent
{
    public Guid HeadOfFamilyPersonId { get; set; }
    public string MemberName { get; set; } = string.Empty;

    public FamilyMemberStatusChangedEvent(Guid headOfFamilyPersonId, string memberName)
    {
        HeadOfFamilyPersonId = headOfFamilyPersonId;
        MemberName = memberName;
    }
}
