namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a login attempt is made from an unrecognized or new device.
/// </summary>
public class NewDeviceLoginAttemptEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the user attempting to log in.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the name or description of the device.
    /// </summary>
    public string DeviceName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="NewDeviceLoginAttemptEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="deviceName">The reported name of the device.</param>
    public NewDeviceLoginAttemptEvent(Guid userId, string deviceName)
    {
        UserId = userId;
        DeviceName = deviceName;
    }
}
