namespace Huwiyati.Application.Organizations.Branches.Commands;

public class CreateBranchCommand
{
    public Guid OrganizationId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string Governorate { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string? AddressDetails { get; set; }
    public string? PhoneNumber { get; set; }
}