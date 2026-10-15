namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to reactivate a previously deactivated citizen account using credentials and OTP verification.
/// </summary>
public class ReactivateAccountCommand
{
    /// <summary>
    /// Gets or sets the national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the account password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the one-time passcode (OTP) confirming reactivation.
    /// </summary>
    public string Code { get; set; } = string.Empty;
}
