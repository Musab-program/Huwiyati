namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

public class EmployeeAssignedEvent : BaseEvent
{
    public Guid UserId { get; set; }
    public string BranchName { get; set; } = string.Empty;

    public EmployeeAssignedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
