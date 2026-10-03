namespace Huwiyati.Domain.Events.Requests;

using Huwiyati.Domain.Common;

public class ServiceRequestCreatedEvent : BaseEvent
{
    public Guid UserId { get; set; }
    public string RequestNumber { get; set; } = string.Empty;

    public ServiceRequestCreatedEvent(Guid userId, string requestNumber)
    {
        UserId = userId;
        RequestNumber = requestNumber;
    }
}
