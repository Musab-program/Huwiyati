namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to request changing the authenticated citizen's registered phone number.
/// </summary>
public class ChangePhoneCommand
{
    /// <summary>
    /// Gets or sets the current account password to authenticate the change request.
    /// </summary>
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new phone number to be assigned to the user account.
    /// </summary>
    public string NewPhoneNumber { get; set; } = string.Empty;
}
