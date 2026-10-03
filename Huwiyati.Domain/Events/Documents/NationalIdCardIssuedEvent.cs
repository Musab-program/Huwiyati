namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

public class NationalIdCardIssuedEvent : BaseEvent
{
    public Guid PersonId { get; set; }
    public string CardNumber { get; set; } = string.Empty;

    public NationalIdCardIssuedEvent(Guid personId, string cardNumber)
    {
        PersonId = personId;
        CardNumber = cardNumber;
    }
}
