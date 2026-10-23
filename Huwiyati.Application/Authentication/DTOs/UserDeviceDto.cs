namespace Huwiyati.Application.Authentication.DTOs;

/// <summary>
/// Data transfer object representing a registered user hardware device or client browser.
/// </summary>
public class UserDeviceDto
{
    /// <summary>
    /// Gets or sets the unique primary key identifier of the device record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique hardware fingerprint or persistent browser UUID.
    /// </summary>
    public string DeviceIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the friendly name of the device (e.g., iPhone 15, Chrome Windows).
    /// </summary>
    public string DeviceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the operating system and platform information.
    /// </summary>
    public string OperatingSystem { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this device has passed OTP verification and is trusted.
    /// </summary>
    public bool IsTrusted { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp of the most recent successful login from this device.
    /// </summary>
    public DateTime? LastLogin { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the device was first registered.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
