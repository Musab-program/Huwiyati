namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an employee's details or branch assignment have been updated.
/// </summary>
public class EmployeeUpdatedEvent : BaseEvent
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
    /// Initializes a new instance of the <see cref="EmployeeUpdatedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the employee user.</param>
    /// <param name="branchName">The name of the branch.</param>
    public EmployeeUpdatedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
