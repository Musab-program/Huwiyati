namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

public class PersonDataUpdatedEvent : BaseEvent
{
    public Guid PersonId { get; set; }

    public PersonDataUpdatedEvent(Guid personId)
    {
        PersonId = personId;
    }
}
