namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an administrator's profile or assignment details have been updated.
/// </summary>
public class AdminUpdatedEvent : BaseEvent
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
    /// Initializes a new instance of the <see cref="AdminUpdatedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the administrator user.</param>
    /// <param name="branchName">The name of the branch.</param>
    public AdminUpdatedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
