namespace Huwiyati.Application.Requests.DTOs;

using Huwiyati.Domain.Enums;

/// <summary>
/// Represents a historical status change entry for a service request.
/// </summary>
public class RequestStatusHistoryDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the status history record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the service request tracking number.
    /// </summary>
    public string RequestNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the associated service request.
    /// </summary>
    public Guid ServiceRequestId { get; set; }

    /// <summary>
    /// Gets or sets the status transitioned into.
    /// </summary>
    public RequestStatus Status { get; set; }

    /// <summary>
    /// Gets or sets an optional note or rationale explaining the status change.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the status transition was recorded.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user or system actor who initiated the status change.
    /// </summary>
    public string? CreatedBy { get; set; }
}
