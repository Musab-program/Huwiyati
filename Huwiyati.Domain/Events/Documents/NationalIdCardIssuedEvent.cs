namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a new national identity card has been issued to a citizen.
/// </summary>
public class NationalIdCardIssuedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen recipient.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the national identification card number.
    /// </summary>
    public string CardNumber { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="NationalIdCardIssuedEvent"/> class.
    /// </summary>
    /// <param name="personId">The unique identifier of the citizen.</param>
    /// <param name="cardNumber">The issued national ID card number.</param>
    public NationalIdCardIssuedEvent(Guid personId, string cardNumber)
    {
        PersonId = personId;
        CardNumber = cardNumber;
    }
}
