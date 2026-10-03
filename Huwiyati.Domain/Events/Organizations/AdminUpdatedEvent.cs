namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

public class AdminUpdatedEvent : BaseEvent
{
    public Guid UserId { get; set; }
    public string BranchName { get; set; } = string.Empty;

    public AdminUpdatedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
