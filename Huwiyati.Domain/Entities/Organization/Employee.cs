namespace Huwiyati.Domain.Entities.Organizations;

using Huwiyati.Domain.Common;

/// <summary>
/// Represents an administrative staff member or agent employed at an organization branch.
/// </summary>
public class Employee : AuditableEntity
{
    /// <summary>
    /// Gets or sets the foreign key referencing the underlying Identity user account.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the branch where the employee is assigned.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the official employee identification/badge number.
    /// </summary>
    public string EmployeeNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the employee is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the organization branch navigation entity.
    /// </summary>
    public OrganizationBranch Branch { get; set; } = null!;
}
