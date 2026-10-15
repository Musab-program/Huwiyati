namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to complete password reset using a verified OTP code.
/// </summary>
public class ResetPasswordCommand
{
    /// <summary>
    /// Gets or sets the national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the OTP verification code delivered to the user's email.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the newly chosen password.
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;
}