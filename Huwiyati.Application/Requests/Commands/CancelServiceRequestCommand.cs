namespace Huwiyati.Application.Requests.Commands;

// Command for applicants to cancel a pending service request
public class CancelServiceRequestCommand
{
    public Guid ServiceRequestId { get; set; }
    public string? Reason { get; set; }
}
