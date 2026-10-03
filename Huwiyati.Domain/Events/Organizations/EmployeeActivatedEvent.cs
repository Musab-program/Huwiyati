namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

public class EmployeeActivatedEvent : BaseEvent
{
    public Guid UserId { get; set; }
    public string BranchName { get; set; } = string.Empty;

    public EmployeeActivatedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
