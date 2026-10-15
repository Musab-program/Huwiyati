namespace Huwiyati.Application.Documents.Passport.Commands;

using Huwiyati.Domain.Enums;

/// <summary>
/// Command payload for issuing a new passport to a citizen for the first time.
/// </summary>
public class IssuePassportCommand
{
    /// <summary>
    /// Gets or sets the 11-digit national number of the citizen requesting the passport.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional updated biometric photo URL.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the passport category (defaults to <see cref="PassportType.Regular"/>).
    /// </summary>
    public PassportType PassportType { get; set; } = PassportType.Regular;

    /// <summary>
    /// Gets or sets the immigration and passport authority branch identifier where issued.
    /// </summary>
    public Guid IssuingBranchId { get; set; }
}
