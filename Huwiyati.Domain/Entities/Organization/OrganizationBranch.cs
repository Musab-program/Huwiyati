namespace Huwiyati.Domain.Entities.Organizations;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.Documents;

/// <summary>
/// Represents a physical branch, office, or service center of an organization.
/// </summary>
public class OrganizationBranch : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the parent organization.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the official branch name.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the governorate (province) where the branch is located.
    /// </summary>
    public string Governorate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the district or municipality where the branch is located.
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional street address or location details.
    /// </summary>
    public string? AddressDetails { get; set; }

    /// <summary>
    /// Gets or sets the official contact phone number for the branch.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the branch is currently active and accepting requests.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the parent organization navigation entity.
    /// </summary>
    public Organization Organization { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of employees assigned to this branch.
    /// </summary>
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    /// <summary>
    /// Gets or sets the collection of national ID cards issued by this branch.
    /// </summary>
    public ICollection<NationalIdCard> NationalIdCards { get; set; } = new List<NationalIdCard>();
}