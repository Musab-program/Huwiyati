namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to verify whether a password reset OTP code is valid prior to displaying the new password input form.
/// </summary>
public class VerifyResetCommand
{
    /// <summary>
    /// Gets or sets the national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the 6-digit verification code.
    /// </summary>
    public string Code { get; set; } = string.Empty;
}