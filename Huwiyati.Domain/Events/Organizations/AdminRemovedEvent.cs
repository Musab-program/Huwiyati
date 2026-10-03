namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

public class AdminRemovedEvent : BaseEvent
{
    public Guid UserId { get; set; }
    public string BranchName { get; set; } = string.Empty;

    public AdminRemovedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
