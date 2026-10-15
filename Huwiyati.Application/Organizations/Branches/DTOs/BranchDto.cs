namespace Huwiyati.Application.Organizations.Branches.DTOs;

/// <summary>
/// Represents data transfer object containing organization branch details.
/// </summary>
public class BranchDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the branch.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the owning organization.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the name of the owning organization.
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the branch office.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the governorate where the branch is located.
    /// </summary>
    public string Governorate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the district where the branch is located.
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets optional detailed physical address information.
    /// </summary>
    public string? AddressDetails { get; set; }

    /// <summary>
    /// Gets or sets the contact telephone number for the branch.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the branch is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the branch record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}