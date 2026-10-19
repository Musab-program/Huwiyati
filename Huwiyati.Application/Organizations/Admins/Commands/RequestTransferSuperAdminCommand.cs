namespace Huwiyati.Application.Organizations.Admins.Commands;

/// <summary>
/// Command to request transferring SuperAdmin role by validating credentials and generating an OTP code.
/// </summary>
public class RequestTransferSuperAdminCommand
{
    /// <summary>
    /// Gets or sets the national number of the executing SuperAdmin.
    /// </summary>
    public string SuperAdminNationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current password of the executing SuperAdmin.
    /// </summary>
    public string CurrentPassword { get; set; } = string.Empty;
}
