namespace Huwiyati.Application.Requests.DTOs;

/// <summary>
/// Represents data transfer object containing service type definition and catalog details.
/// </summary>
public class ServiceTypeDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the service type.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the display name of the service type.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique code identifying the service type.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the owning organization.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the name of the owning organization.
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the service type is active and available for requests.
    /// </summary>
    public bool IsActive { get; set; }
}
