namespace Huwiyati.Application.Documents.Passport.Commands;

using Huwiyati.Domain.Enums;

/// <summary>
/// Command payload for renewing an existing passport approaching expiration or expired.
/// </summary>
public class RenewPassportCommand
{
    /// <summary>
    /// Gets or sets the 11-digit national number of the passport holder.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the immigration and passport branch identifier processing the renewal.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets an optional updated biometric photo URL.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets an optional updated passport category.
    /// </summary>
    public PassportType? PassportType { get; set; }
}
