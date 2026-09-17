namespace Huwiyati.Application.Family.DTOs;
// Data Transfer Object representing Family record and Family Card details returned to client
public class FamilyDto
{
    public Guid Id { get; set; }
    public string FamilyNumber { get; set; } = string.Empty;
    public Guid HeadOfFamilyPersonId { get; set; }
    public string HeadOfFamilyNationalNumber { get; set; } = string.Empty;
    public string HeadOfFamilyFullName { get; set; } = string.Empty;
    public Guid IssuingBranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public string? QrCodePayload { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<FamilyMemberDto> Members { get; set; } = new List<FamilyMemberDto>();
}

