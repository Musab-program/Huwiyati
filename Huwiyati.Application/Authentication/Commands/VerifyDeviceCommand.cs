namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to complete device verification by submitting the OTP received for an unrecognized device.
/// </summary>
public class VerifyDeviceCommand
{
    /// <summary>
    /// Gets or sets the citizen or employee national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hardware or fingerprint device identifier being verified.
    /// </summary>
    public string DeviceIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the one-time passcode (OTP) received by the user.
    /// </summary>
    public string Code { get; set; } = string.Empty;
}