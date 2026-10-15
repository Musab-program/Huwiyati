namespace Huwiyati.Application.Authentication.DTOs;

using Huwiyati.Domain.Enums;

/// <summary>
/// Data transfer object representing citizen account activation and registration status details for administrative staff.
/// </summary>
public class CitizenAccountStatusDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen in the civil registry.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the full quadruple name of the citizen.
    /// </summary>
    public string PersonFullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the national identification number.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether an Identity login account exists for this citizen.
    /// </summary>
    public bool HasRegisteredAccount { get; set; }

    /// <summary>
    /// Gets or sets the linked Identity user identifier, if registered.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the registered email address, if available.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the registered contact phone number, if available.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the current operational status of the account, if registered.
    /// </summary>
    public AccountStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the account was registered.
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the account was activated.
    /// </summary>
    public DateTime? ActivatedAt { get; set; }
}
