namespace Huwiyati.Application.Family.DTOs;

// Lightweight summary DTO for list / table views of Family records
public class FamilySummaryDto
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
    public string Status { get; set; } = string.Empty;
    public int ActiveMembersCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
