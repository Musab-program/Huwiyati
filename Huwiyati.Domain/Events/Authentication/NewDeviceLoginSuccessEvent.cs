namespace Huwiyati.Domain.Events.Authentication;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a user successfully completes a login from a newly registered device.
/// </summary>
public class NewDeviceLoginSuccessEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the user who logged in.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the name or model description of the new device.
    /// </summary>
    public string DeviceName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="NewDeviceLoginSuccessEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the authenticated user.</param>
    /// <param name="deviceName">The verified device name.</param>
    public NewDeviceLoginSuccessEvent(Guid userId, string deviceName)
    {
        UserId = userId;
        DeviceName = deviceName;
    }
}
