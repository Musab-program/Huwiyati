namespace Huwiyati.Application.Organizations.Branches.DTOs;

public class BranchDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string OrganizationName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string Governorate { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string? AddressDetails { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}