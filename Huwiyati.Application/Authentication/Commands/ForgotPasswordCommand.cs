namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to initiate a password reset workflow for a citizen account.
/// </summary>
public class ForgotPasswordCommand
{
    /// <summary>
    /// Gets or sets the national identification number of the account holder.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;
}