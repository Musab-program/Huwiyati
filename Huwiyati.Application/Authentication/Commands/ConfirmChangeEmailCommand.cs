namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to confirm changing the authenticated citizen's registered email address using the verification code.
/// </summary>
public class ConfirmChangeEmailCommand
{
    /// <summary>
    /// Gets or sets the new email address being confirmed.
    /// </summary>
    public string NewEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the 6-digit OTP verification code received on the new email.
    /// </summary>
    public string Code { get; set; } = string.Empty;
}
