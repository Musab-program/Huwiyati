namespace Huwiyati.Domain.Entities.Requests;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Enums;

/// <summary>
/// Represents an audit trail entry tracking a status transition for a service request.
/// </summary>
public class RequestStatusHistory : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the associated service request.
    /// </summary>
    public Guid ServiceRequestId { get; set; }

    /// <summary>
    /// Gets or sets the status to which the request transitioned.
    /// </summary>
    public RequestStatus Status { get; set; }

    /// <summary>
    /// Gets or sets an optional administrative note or explanatory remark regarding the transition.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Gets or sets the parent service request navigation entity.
    /// </summary>
    public ServiceRequest ServiceRequest { get; set; } = null!;
}
