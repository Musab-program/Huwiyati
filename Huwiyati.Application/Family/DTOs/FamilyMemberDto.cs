namespace Huwiyati.Application.Family.DTOs;

// Data Transfer Object representing an individual family member within the Family Card
public class FamilyMemberDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string NationalNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string RelationshipType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Guid? MarriageContractId { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
}