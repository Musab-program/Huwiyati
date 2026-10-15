namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an employee has been assigned to an organization branch.
/// </summary>
public class EmployeeAssignedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the employee user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the assigned branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeAssignedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the employee user.</param>
    /// <param name="branchName">The name of the branch.</param>
    public EmployeeAssignedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
