namespace Huwiyati.Domain.Entities.Documents;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Enums;

/// <summary>
/// Represents an official passport travel document issued to a citizen.
/// </summary>
public class Passport : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen holding the passport.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the passport office/branch that issued the document.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the unique passport number.
    /// </summary>
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of passport (e.g., Regular, Diplomatic, Special, Service).
    /// </summary>
    public PassportType PassportType { get; set; } = PassportType.Regular;

    /// <summary>
    /// Gets or sets the date the passport was issued.
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the date the passport expires.
    /// </summary>
    public DateOnly ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the encrypted or structured QR code payload for verification purposes.
    /// </summary>
    public string? QrCodePayload { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the operational status of the passport (e.g., Active, Expired, Revoked, Lost).
    /// </summary>
    public PassportStatus Status { get; set; } = PassportStatus.Active;

    /// <summary>
    /// Gets or sets the citizen navigation entity.
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// Gets or sets the issuing branch navigation entity.
    /// </summary>
    public OrganizationBranch IssuingBranch { get; set; } = null!;
}
