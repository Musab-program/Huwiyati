namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

public class PasswordResetSuccessEvent : BaseEvent
{
    public Guid UserId { get; set; }

    public PasswordResetSuccessEvent(Guid userId)
    {
        UserId = userId;
    }
}
