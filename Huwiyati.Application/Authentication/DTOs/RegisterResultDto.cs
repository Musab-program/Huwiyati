namespace Huwiyati.Application.Authentication.DTOs;

/// <summary>
/// Data transfer object returned following citizen registration submission.
/// </summary>
public class RegisterResultDto
{
    /// <summary>
    /// Gets or sets the newly provisioned Identity user identifier.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the national identification number associated with the account.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the initial account status (typically "PendingActivation").
    /// </summary>
    public string AccountStatus { get; set; } = string.Empty;
}
