namespace Huwiyati.Domain.Entities.Family;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Enums;

// Represents a Family registry record and Family Card
public class Family : AuditableEntity
{
    public string FamilyNumber { get; set; } = string.Empty;
    public Guid HeadOfFamilyPersonId { get; set; }
    public Guid IssuingBranchId { get; set; }
    public DateOnly IssueDate { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public string? QrCodePayload { get; set; }
    public FamilyStatus Status { get; set; } = FamilyStatus.Active;

    // Navigation properties
    public Person HeadOfFamily { get; set; } = null!;
    public OrganizationBranch IssuingBranch { get; set; } = null!;
    public ICollection<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();
}
