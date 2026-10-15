namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to revoke trust for a registered device associated with a user account.
/// </summary>
public class RemoveDeviceCommand
{
    /// <summary>
    /// Gets or sets the national identification number of the user.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique hardware or fingerprint identifier of the device to revoke.
    /// </summary>
    public string DeviceIdentifier { get; set; } = string.Empty;
}