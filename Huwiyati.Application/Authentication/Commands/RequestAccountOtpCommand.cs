namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to request a temporary one-time password (OTP) for account-level security operations (e.g. deactivation, reactivation).
/// </summary>
public class RequestAccountOtpCommand
{
    /// <summary>
    /// Gets or sets the national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the account password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
