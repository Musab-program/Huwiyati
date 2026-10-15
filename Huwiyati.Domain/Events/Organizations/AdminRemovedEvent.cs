namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an administrator has been removed from a branch.
/// </summary>
public class AdminRemovedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the administrator user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the branch from which the admin was removed.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminRemovedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the administrator user.</param>
    /// <param name="branchName">The name of the branch.</param>
    public AdminRemovedEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
