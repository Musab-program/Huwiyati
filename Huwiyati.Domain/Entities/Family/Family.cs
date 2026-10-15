namespace Huwiyati.Domain.Entities.Family;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Enums;

/// <summary>
/// Represents a family registry record and the associated family card document in the civil registry.
/// </summary>
public class Family : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique nationwide family registry or family book number.
    /// </summary>
    public string FamilyNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the citizen designated as the head of family.
    /// </summary>
    public Guid HeadOfFamilyPersonId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the civil registry branch that issued the family record.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the official issue date of the family card.
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the family card.
    /// </summary>
    public DateOnly ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the encrypted or structured QR code payload for verification purposes.
    /// </summary>
    public string? QrCodePayload { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the family card/record (e.g., Active, Expired, Suspended, Archived).
    /// </summary>
    public FamilyStatus Status { get; set; } = FamilyStatus.Active;

    /// <summary>
    /// Gets or sets the head of family person navigation entity.
    /// </summary>
    public Person HeadOfFamily { get; set; } = null!;

    /// <summary>
    /// Gets or sets the civil registry issuing branch navigation entity.
    /// </summary>
    public OrganizationBranch IssuingBranch { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of family members registered under this family.
    /// </summary>
    public ICollection<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();
}
