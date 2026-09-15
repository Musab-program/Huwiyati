namespace Huwiyati.Domain.Entities.Organizations;

using Huwiyati.Domain.Common;

public class Employee : AuditableEntity
{
    public Guid UserId { get; set; }
    public Guid BranchId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation property
    public OrganizationBranch Branch { get; set; } = null!;
}
