namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to revoke trust for a registered device associated with the authenticated user account.
/// </summary>
public class RemoveDeviceCommand
{
    /// <summary>
    /// Gets or sets the unique hardware or fingerprint identifier of the device to revoke.
    /// </summary>
    public string DeviceIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// Optional legacy parameter for backward compatibility. The identity is resolved securely from JWT.
    /// </summary>
    public string? NationalNumber { get; set; }
}
