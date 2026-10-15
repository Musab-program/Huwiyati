namespace Huwiyati.Domain.Events.Requests;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a service request has been cancelled by the citizen or authority.
/// </summary>
public class ServiceRequestCancelledEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the user who owns or initiated the request.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the public-facing tracking number of the cancelled request.
    /// </summary>
    public string RequestNumber { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRequestCancelledEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="requestNumber">The request tracking number.</param>
    public ServiceRequestCancelledEvent(Guid userId, string requestNumber)
    {
        UserId = userId;
        RequestNumber = requestNumber;
    }
}
