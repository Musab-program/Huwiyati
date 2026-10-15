namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an existing national identity card has been renewed.
/// </summary>
public class NationalIdCardRenewedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen recipient.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the renewed national identification card number.
    /// </summary>
    public string CardNumber { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="NationalIdCardRenewedEvent"/> class.
    /// </summary>
    /// <param name="personId">The unique identifier of the citizen.</param>
    /// <param name="cardNumber">The renewed national ID card number.</param>
    public NationalIdCardRenewedEvent(Guid personId, string cardNumber)
    {
        PersonId = personId;
        CardNumber = cardNumber;
    }
}
