namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a user account has been deactivated.
/// </summary>
public class AccountDeactivatedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the deactivated user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountDeactivatedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose account was deactivated.</param>
    public AccountDeactivatedEvent(Guid userId)
    {
        UserId = userId;
    }
}
