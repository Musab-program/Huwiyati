namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to confirm changing the authenticated user's registered phone number using the verification OTP code.
/// </summary>
public class ConfirmChangePhoneCommand
{
    /// <summary>
    /// Gets or sets the new phone number being confirmed.
    /// </summary>
    public string NewPhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the 6-digit OTP verification code received on the user's registered email.
    /// </summary>
    public string Code { get; set; } = string.Empty;
}
