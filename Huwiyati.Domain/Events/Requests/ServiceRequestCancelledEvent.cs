namespace Huwiyati.Domain.Events.Requests;

using Huwiyati.Domain.Common;

public class ServiceRequestCancelledEvent : BaseEvent
{
    public Guid UserId { get; set; }
    public string RequestNumber { get; set; } = string.Empty;

    public ServiceRequestCancelledEvent(Guid userId, string requestNumber)
    {
        UserId = userId;
        RequestNumber = requestNumber;
    }
}
