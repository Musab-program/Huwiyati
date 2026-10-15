namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an administrator has been assigned to a branch.
/// </summary>
public class AdminAssignedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the administrator user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the assigned branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminAssignedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the administrator user.</param>
    /// <param name="branchName">The name of the branch to which the admin was assigned.</param>
    public AdminAssignedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
