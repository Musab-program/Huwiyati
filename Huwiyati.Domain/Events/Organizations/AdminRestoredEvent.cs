namespace Huwiyati.Domain.Events.Organizations;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an administrator's administrative privileges or branch assignment have been restored.
/// </summary>
public class AdminRestoredEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the administrator user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the branch to which the admin was restored.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminRestoredEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the administrator user.</param>
    /// <param name="branchName">The name of the branch.</param>
    public AdminRestoredEvent(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}
