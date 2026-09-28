namespace Huwiyati.Domain.Entities.CivilRegistry;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Entities.Requests;

public class ServiceType : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid OrganizationId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public Organization Organization { get; set; } = null!;
    public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
}
