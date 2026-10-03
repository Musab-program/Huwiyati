namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

public class EmployeeUpdatedEvent : BaseEvent
{
    public Guid UserId { get; set; }
    public string BranchName { get; set; } = string.Empty;

    public EmployeeUpdatedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
