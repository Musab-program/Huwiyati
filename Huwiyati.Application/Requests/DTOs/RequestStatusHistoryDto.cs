namespace Huwiyati.Application.Requests.DTOs;

using Huwiyati.Domain.Enums;

public class RequestStatusHistoryDto
{
    public Guid Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public Guid ServiceRequestId { get; set; }
    public RequestStatus Status { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
}
