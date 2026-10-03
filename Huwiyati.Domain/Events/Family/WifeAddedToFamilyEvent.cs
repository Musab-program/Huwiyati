namespace Huwiyati.Domain.Events.Family;

using Huwiyati.Domain.Common;

public class WifeAddedToFamilyEvent : BaseEvent
{
    public Guid HeadOfFamilyPersonId { get; set; }
    public Guid WifePersonId { get; set; }
    public string WifeName { get; set; } = string.Empty;

    public WifeAddedToFamilyEvent(Guid headOfFamilyPersonId, Guid wifePersonId, string wifeName)
    {
        HeadOfFamilyPersonId = headOfFamilyPersonId;
        WifePersonId = wifePersonId;
        WifeName = wifeName;
    }
}
