namespace Huwiyati.Domain.Entities.Authentication;

using Huwiyati.Domain.Common;

/// <summary>
/// Represents a hardware device or client browser registered and tracked for user authentication security.
/// Enforces trusted device policies, OTP challenge logic, and multi-device management.
/// </summary>
public class UserDevice : BaseEntity
{
    /// <summary>
    /// Gets or sets the foreign key identifier of the user account owning the device.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the human-readable friendly name of the device (e.g., iPhone 15, Chrome Windows).
    /// </summary>
    public string DeviceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique hardware fingerprint or persistent browser UUID identifier.
    /// </summary>
    public string DeviceIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the operating system and platform details detected for this client.
    /// </summary>
    public string OperatingSystem { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this device has passed OTP verification and is trusted.
    /// </summary>
    public bool IsTrusted { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp of the most recent successful login originating from this device.
    /// </summary>
    public DateTime? LastLogin { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the device was first registered in the system.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}