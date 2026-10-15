namespace Huwiyati.Domain.Entities.Family;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Enums;

/// <summary>
/// Represents an individual member registered within a family registry unit.
/// </summary>
public class FamilyMember : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the parent family record.
    /// </summary>
    public Guid FamilyId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the person registered as a member.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the optional unique identifier of the marriage contract linking a spouse to the family.
    /// </summary>
    public Guid? MarriageContractId { get; set; }

    /// <summary>
    /// Gets or sets the relationship of this member to the head of family (e.g., Wife, Son, Daughter, Dependent).
    /// </summary>
    public RelationshipType RelationshipType { get; set; }

    /// <summary>
    /// Gets or sets the membership status (e.g., Active, Separated, Deceased, Transferred).
    /// </summary>
    public FamilyMemberStatus Status { get; set; } = FamilyMemberStatus.Active;

    /// <summary>
    /// Gets or sets the UTC date and time when the member was added to the family.
    /// </summary>
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the UTC date and time when the member left or separated from the family, if applicable.
    /// </summary>
    public DateTime? LeftAt { get; set; }

    /// <summary>
    /// Gets or sets the parent family navigation entity.
    /// </summary>
    public Family Family { get; set; } = null!;

    /// <summary>
    /// Gets or sets the person navigation entity.
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// Gets or sets the marriage contract navigation entity, if applicable.
    /// </summary>
    public MarriageContract? MarriageContract { get; set; }
}
