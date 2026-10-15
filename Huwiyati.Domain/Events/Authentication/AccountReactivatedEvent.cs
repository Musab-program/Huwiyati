namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a previously deactivated user account has been reactivated.
/// </summary>
public class AccountReactivatedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the reactivated user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountReactivatedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose account was reactivated.</param>
    public AccountReactivatedEvent(Guid userId)
    {
        UserId = userId;
    }
}
