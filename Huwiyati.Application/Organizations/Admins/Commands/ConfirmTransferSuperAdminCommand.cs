namespace Huwiyati.Application.Organizations.Admins.Commands;

/// <summary>
/// Command to confirm transferring SuperAdmin role to a target citizen using OTP verification.
/// </summary>
public class ConfirmTransferSuperAdminCommand
{
    /// <summary>
    /// Gets or sets the national number of the citizen being elevated to SuperAdmin.
    /// </summary>
    public string TargetNationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the 6-digit OTP code received via email.
    /// </summary>
    public string OtpCode { get; set; } = string.Empty;
}
