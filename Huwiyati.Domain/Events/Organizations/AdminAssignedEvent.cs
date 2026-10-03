namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

public class AdminAssignedEvent : BaseEvent
{
    public Guid UserId { get; set; }
    public string BranchName { get; set; } = string.Empty;

    public AdminAssignedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
