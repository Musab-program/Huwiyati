namespace Huwiyati.Domain.Entities.CivilRegistry;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Entities.Requests;

/// <summary>
/// Represents a governmental civil service category (e.g., National ID Renewal, Passport Renewal).
/// Defines metadata, unique system service codes, and organizational ownership.
/// </summary>
public class ServiceType : AuditableEntity
{
    /// <summary>
    /// Gets or sets the Arabic title of the civil service.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique business identification code for the service (e.g., NATIONAL_ID_RENEWAL).
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key identifier of the parent government organization offering this service.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this service is currently active and available for submission.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the navigation property to the parent government organization.
    /// </summary>
    public Organization Organization { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of service requests initiated under this service category.
    /// </summary>
    public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
}
