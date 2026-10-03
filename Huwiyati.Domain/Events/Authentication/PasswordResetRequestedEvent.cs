namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

public class PasswordResetRequestedEvent : BaseEvent
{
    public Guid UserId { get; set; }

    public PasswordResetRequestedEvent(Guid userId)
    {
        UserId = userId;
    }
}
