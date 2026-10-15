namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to voluntarily deactivate a user account with verification code and password.
/// </summary>
public class DeactivateAccountCommand
{
    /// <summary>
    /// Gets or sets the citizen's national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's password for re-authentication.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the one-time passcode (OTP) confirming the deactivation intent.
    /// </summary>
    public string Code { get; set; } = string.Empty;
}
