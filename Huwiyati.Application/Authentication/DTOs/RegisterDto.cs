namespace Huwiyati.Application.Authentication.DTOs;

/// <summary>
/// Data transfer object containing citizen initial account registration parameters.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Gets or sets the national identification number verified against the Civil Registry.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of birth matching the Civil Registry record.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the citizen's contact phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the citizen's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the desired account password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
