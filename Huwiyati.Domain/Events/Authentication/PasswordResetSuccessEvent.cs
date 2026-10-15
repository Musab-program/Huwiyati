namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a user account password has been successfully reset.
/// </summary>
public class PasswordResetSuccessEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the user whose password was reset.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordResetSuccessEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    public PasswordResetSuccessEvent(Guid userId)
    {
        UserId = userId;
    }
}
