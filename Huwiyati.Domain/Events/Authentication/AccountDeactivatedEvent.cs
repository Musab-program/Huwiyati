namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

public class AccountDeactivatedEvent : BaseEvent
{
    public Guid UserId { get; set; }

    public AccountDeactivatedEvent(Guid userId)
    {
        UserId = userId;
    }
}
