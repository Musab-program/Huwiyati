namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

public class AccountReactivatedEvent : BaseEvent
{
    public Guid UserId { get; set; }

    public AccountReactivatedEvent(Guid userId)
    {
        UserId = userId;
    }
}
