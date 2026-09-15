namespace Huwiyati.Domain.Entities.Organizations;

using Huwiyati.Domain.Common;

public class Organization : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation property
    public ICollection<OrganizationBranch> Branches { get; set; } = new List<OrganizationBranch>();
}