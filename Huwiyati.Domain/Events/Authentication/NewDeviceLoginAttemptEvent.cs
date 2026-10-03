namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

public class NewDeviceLoginAttemptEvent : BaseEvent
{
    public Guid UserId { get; set; }
    public string DeviceName { get; set; } = string.Empty;

    public NewDeviceLoginAttemptEvent(Guid userId, string deviceName)
    {
        UserId = userId;
        DeviceName = deviceName;
    }
}
