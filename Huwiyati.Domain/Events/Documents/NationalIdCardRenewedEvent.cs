namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

public class NationalIdCardRenewedEvent : BaseEvent
{
    public Guid PersonId { get; set; }
    public string CardNumber { get; set; } = string.Empty;

    public NationalIdCardRenewedEvent(Guid personId, string cardNumber)
    {
        PersonId = personId;
        CardNumber = cardNumber;
    }
}
