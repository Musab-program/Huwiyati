namespace Huwiyati.Domain.Events.Requests;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a service request transitions to a new status (e.g., InReview, Approved, Issued, Rejected).
/// </summary>
public class ServiceRequestStatusChangedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the user who owns the request.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the public-facing tracking number of the request.
    /// </summary>
    public string RequestNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name or representation of the new status.
    /// </summary>
    public string NewStatus { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRequestStatusChangedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="requestNumber">The request tracking number.</param>
    /// <param name="newStatus">The string representation of the new status.</param>
    public ServiceRequestStatusChangedEvent(Guid userId, string requestNumber, string newStatus)
    {
        UserId = userId;
        RequestNumber = requestNumber;
        NewStatus = newStatus;
    }
}
