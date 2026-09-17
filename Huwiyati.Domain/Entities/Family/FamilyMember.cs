namespace Huwiyati.Domain.Entities.Family;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Enums;

// Represents an individual member belonging to a Family registry
public class FamilyMember : AuditableEntity
{
    public Guid FamilyId { get; set; }
    public Guid PersonId { get; set; }
    public Guid? MarriageContractId { get; set; }
    public RelationshipType RelationshipType { get; set; }
    public FamilyMemberStatus Status { get; set; } = FamilyMemberStatus.Active;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LeftAt { get; set; }

    // Navigation properties
    public Family Family { get; set; } = null!;
    public Person Person { get; set; } = null!;
    public MarriageContract? MarriageContract { get; set; }
}
