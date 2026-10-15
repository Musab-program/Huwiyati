namespace Huwiyati.Domain.Entities.Organizations;

using Huwiyati.Domain.Common;

/// <summary>
/// Represents a governmental authority or organizational entity (e.g., Civil Registry Authority, Immigration and Passport Authority).
/// </summary>
public class Organization : BaseEntity
{
    /// <summary>
    /// Gets or sets the official name of the organization.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the organization is active in the system.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the collection of regional and local branches operating under this organization.
    /// </summary>
    public ICollection<OrganizationBranch> Branches { get; set; } = new List<OrganizationBranch>();
}