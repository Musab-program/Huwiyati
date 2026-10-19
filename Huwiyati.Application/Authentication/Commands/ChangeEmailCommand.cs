namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to request changing the authenticated citizen's registered email address.
/// </summary>
public class ChangeEmailCommand
{
    /// <summary>
    /// Gets or sets the current account password to authenticate the change request.
    /// </summary>
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new email address to be assigned to the account.
    /// </summary>
    public string NewEmail { get; set; } = string.Empty;
}
