namespace Huwiyati.Application.Organizations.Branches.Commands;

/// <summary>
/// Command to create a new branch under a specified organization.
/// </summary>
public class CreateBranchCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the owning organization.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the name of the new branch office.
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
    /// Gets or sets optional contact telephone number for the branch.
    /// </summary>
    public string? PhoneNumber { get; set; }
}