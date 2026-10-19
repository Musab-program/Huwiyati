namespace Huwiyati.Application.Authentication.Commands;

using System;

/// <summary>
/// Command to authenticate a user account under a specified role and device context.
/// </summary>
public class LoginCommand
{
    /// <summary>
    /// Gets or sets the citizen or employee national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the plain-text password to verify.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target system role being requested for this session.
    /// </summary>
    public string RequestedRole { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional organization identifier when authenticating staff through a specific organization portal.
    /// </summary>
    public Guid? OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the unique hardware or fingerprint device identifier.
    /// </summary>
    public string DeviceIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user-friendly name of the client device.
    /// </summary>
    public string DeviceName { get; set; } = "Unknown Device";

    /// <summary>
    /// Gets or sets the client operating system and version.
    /// </summary>
    public string OperatingSystem { get; set; } = "Unknown OS";
}
