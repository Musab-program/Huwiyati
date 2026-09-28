namespace Huwiyati.Application.Requests.Commands;

using Huwiyati.Domain.Enums;

// Command for employees to process and transit a ServiceRequest state
public class ChangeServiceRequestStatusCommand
{
    public Guid ServiceRequestId { get; set; }
    public RequestStatus NewStatus { get; set; }
    public string? Note { get; set; }
    public string? RejectionReason { get; set; }
}
