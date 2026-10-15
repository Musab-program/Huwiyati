namespace Huwiyati.Application.Authentication.DTOs;

using Huwiyati.Domain.Enums;

/// <summary>
/// Data transfer object representing a registered application user account for administrative reporting and querying.
/// </summary>
public class RegisteredUserAccountDto
{
    /// <summary>
    /// Gets or sets the Identity user unique identifier.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the linked civil registry person identifier.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the citizen's national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full quadruple name of the citizen.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the user's phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the operational account status.
    /// </summary>
    public AccountStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the account was registered.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the account was activated.
    /// </summary>
    public DateTime? ActivatedAt { get; set; }
}
