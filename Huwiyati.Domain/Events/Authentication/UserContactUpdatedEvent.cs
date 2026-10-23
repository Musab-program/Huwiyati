namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a user account contact detail (email or phone number) has been successfully updated.
/// </summary>
public class UserContactUpdatedEvent : BaseEvent
{
    /// <summary>
    /// Gets the unique identifier of the user account.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the type of contact updated (e.g., "Email" or "PhoneNumber").
    /// </summary>
    public string ContactType { get; }

    /// <summary>
    /// Gets the new updated contact value.
    /// </summary>
    public string NewValue { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserContactUpdatedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="contactType">The contact attribute name ("Email" or "PhoneNumber").</param>
    /// <param name="newValue">The newly verified value.</param>
    public UserContactUpdatedEvent(Guid userId, string contactType, string newValue)
    {
        UserId = userId;
        ContactType = contactType;
        NewValue = newValue;
    }
}
