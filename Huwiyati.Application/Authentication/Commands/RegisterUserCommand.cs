namespace Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Command to register a new citizen online account verified against official Civil Registry data.
/// </summary>
public class RegisterUserCommand
{
    /// <summary>
    /// Gets or sets the citizen's official national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of birth to cross-reference against civil registry records.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the citizen's contact phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the citizen's contact email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the citizen's desired account password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
