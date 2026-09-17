namespace Huwiyati.Domain.Entities.Family;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Enums;

// Represents an official Marriage Contract document between Husband and Wife
public class MarriageContract : AuditableEntity
{
    public string ContractNumber { get; set; } = string.Empty;
    public Guid HusbandPersonId { get; set; }
    public Guid WifePersonId { get; set; }
    public DateOnly MarriageDate { get; set; }
    public string? DocumentPhotoUrl { get; set; }
    public MarriageStatus Status { get; set; } = MarriageStatus.Active;
    public Guid ApprovedByUserId { get; set; }

    // Navigation properties
    public Person HusbandPerson { get; set; } = null!;
    public Person WifePerson { get; set; } = null!;
}
