namespace Huwiyati.Application.Organizations.Branches.Commands;

/// <summary>
/// Command to update the details of an existing organization branch.
/// </summary>
public class UpdateBranchCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the branch to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the parent organization.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the updated name of the branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated governorate of the branch.
    /// </summary>
    public string Governorate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated district of the branch.
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets optional updated detailed address information.
    /// </summary>
    public string? AddressDetails { get; set; }

    /// <summary>
    /// Gets or sets optional updated phone number for the branch.
    /// </summary>
    public string? PhoneNumber { get; set; }
}