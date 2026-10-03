namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

public class EmployeeDeactivatedEvent : BaseEvent
{
    public Guid UserId { get; set; }
    public string BranchName { get; set; } = string.Empty;

    public EmployeeDeactivatedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
