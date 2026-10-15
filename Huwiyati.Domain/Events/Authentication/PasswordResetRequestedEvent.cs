namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a password reset request has been initiated for a user account.
/// </summary>
public class PasswordResetRequestedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the user requesting password reset.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordResetRequestedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    public PasswordResetRequestedEvent(Guid userId)
    {
        UserId = userId;
    }
}
