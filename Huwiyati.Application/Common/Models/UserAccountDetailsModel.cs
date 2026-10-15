namespace Huwiyati.Application.Common.Models;

using Huwiyati.Domain.Enums;

/// <summary>
/// Encapsulates detailed user account information associated with a civil registry person profile.
/// </summary>
public class UserAccountDetailsModel
{
    /// <summary>
    /// Gets or sets the unique user account identifier in Identity.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the linked civil registry person identifier.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the user's registered email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's registered contact phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current operational account status.
    /// </summary>
    public AccountStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the account was registered.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the account was activated, if active.
    /// </summary>
    public DateTime? ActivatedAt { get; set; }
}
