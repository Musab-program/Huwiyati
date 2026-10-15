namespace Huwiyati.Application.Requests.Commands;

/// <summary>
/// Command for applicants to cancel a pending civil service request.
/// </summary>
public class CancelServiceRequestCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the service request to be cancelled.
    /// </summary>
    public Guid ServiceRequestId { get; set; }

    /// <summary>
    /// Gets or sets an optional cancellation rationale or explanation.
    /// </summary>
    public string? Reason { get; set; }
}
