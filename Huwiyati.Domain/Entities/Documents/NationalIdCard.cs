using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Enums;

namespace Huwiyati.Domain.Entities.Documents;

/// <summary>
/// Represents a citizen's official national identification card.
/// </summary>
public class NationalIdCard : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen holding the card.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the branch that issued the card.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the official issue date of the card.
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the card.
    /// </summary>
    public DateOnly ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the encrypted or structured QR code payload for verification purposes.
    /// </summary>
    public string? QrCodePayload { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the card (e.g., Active, Expired, Suspended, Lost).
    /// </summary>
    public NationalIdCardStatus Status { get; set; } = NationalIdCardStatus.Active;

    /// <summary>
    /// Gets or sets the citizen navigation entity.
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// Gets or sets the issuing organization branch navigation entity.
    /// </summary>
    public OrganizationBranch OrganizationBranch { get; set; } = null!;
}
