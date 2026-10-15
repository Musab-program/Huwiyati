namespace Huwiyati.Application.Requests.Commands;

using Huwiyati.Domain.Enums;

/// <summary>
/// Command for authorized employees to transition the status of a service request.
/// </summary>
public class ChangeServiceRequestStatusCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the target service request.
    /// </summary>
    public Guid ServiceRequestId { get; set; }

    /// <summary>
    /// Gets or sets the new status to apply to the service request.
    /// </summary>
    public RequestStatus NewStatus { get; set; }

    /// <summary>
    /// Gets or sets an optional employee note or internal comment on the status change.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Gets or sets the rejection reason if the new status is <see cref="RequestStatus.Rejected"/>.
    /// </summary>
    public string? RejectionReason { get; set; }
}
