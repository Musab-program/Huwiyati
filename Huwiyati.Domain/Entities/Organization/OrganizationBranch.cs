namespace Huwiyati.Domain.Entities.Organizations;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.Documents;

public class OrganizationBranch : AuditableEntity
{
    public Guid OrganizationId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string Governorate { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string? AddressDetails { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation property
    public Organization Organization { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<NationalIdCard> NationalIdCards { get; set; } = new List<NationalIdCard>();

}