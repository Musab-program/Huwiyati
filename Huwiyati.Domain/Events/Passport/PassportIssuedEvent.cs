namespace Huwiyati.Domain.Events.Passport;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a new passport has been officially issued to a citizen.
/// </summary>
public class PassportIssuedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen recipient.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the issued passport number.
    /// </summary>
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the passport expiration date.
    /// </summary>
    public DateOnly ExpiryDate { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PassportIssuedEvent"/> class.
    /// </summary>
    /// <param name="personId">The unique identifier of the citizen.</param>
    /// <param name="passportNumber">The issued passport number.</param>
    /// <param name="expiryDate">The expiration date of the passport.</param>
    public PassportIssuedEvent(Guid personId, string passportNumber, DateOnly expiryDate)
    {
        PersonId = personId;
        PassportNumber = passportNumber;
        ExpiryDate = expiryDate;
    }
}
