namespace Huwiyati.Domain.Events.Requests;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a new service request has been submitted by a citizen.
/// </summary>
public class ServiceRequestCreatedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the submitting user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the newly assigned tracking number for the request.
    /// </summary>
    public string RequestNumber { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRequestCreatedEvent"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="requestNumber">The request tracking number.</param>
    public ServiceRequestCreatedEvent(Guid userId, string requestNumber)
    {
        UserId = userId;
        RequestNumber = requestNumber;
    }
}
