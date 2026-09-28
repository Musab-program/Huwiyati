namespace Huwiyati.Domain.Entities.Requests;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Enums;

public class RequestStatusHistory : AuditableEntity
{
    public Guid ServiceRequestId { get; set; }
    public RequestStatus Status { get; set; }
    public string? Note { get; set; }

    // Navigation Properties
    public ServiceRequest ServiceRequest { get; set; } = null!;
}
