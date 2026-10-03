namespace Huwiyati.Domain.Events.Requests;

using Huwiyati.Domain.Common;

public class ServiceRequestStatusChangedEvent : BaseEvent
{
    public Guid UserId { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;

    public ServiceRequestStatusChangedEvent(Guid userId, string requestNumber, string newStatus)
    {
        UserId = userId;
        RequestNumber = requestNumber;
        NewStatus = newStatus;
    }
}
