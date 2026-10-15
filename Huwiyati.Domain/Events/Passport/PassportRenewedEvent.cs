namespace Huwiyati.Domain.Events.Passport;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an existing passport has been renewed for a citizen.
/// </summary>
public class PassportRenewedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen recipient.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the renewed passport number.
    /// </summary>
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new expiration date of the renewed passport.
    /// </summary>
    public DateOnly NewExpiryDate { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PassportRenewedEvent"/> class.
    /// </summary>
    /// <param name="personId">The unique identifier of the citizen.</param>
    /// <param name="passportNumber">The renewed passport number.</param>
    /// <param name="newExpiryDate">The new expiration date.</param>
    public PassportRenewedEvent(Guid personId, string passportNumber, DateOnly newExpiryDate)
    {
        PersonId = personId;
        PassportNumber = passportNumber;
        NewExpiryDate = newExpiryDate;
    }
}
