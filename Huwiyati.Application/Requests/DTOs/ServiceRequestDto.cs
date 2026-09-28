namespace Huwiyati.Application.Requests.DTOs;

using Huwiyati.Domain.Enums;

public class ServiceRequestDto
{
    public Guid Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public Guid PersonId { get; set; }
    public string PersonFullName { get; set; } = string.Empty;
    public string NationalNumber { get; set; } = string.Empty;
    public Guid ServiceTypeId { get; set; }
    public string ServiceTypeName { get; set; } = string.Empty;
    public string ServiceTypeCode { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public RequestStatus Status { get; set; }
    public string? RejectionReason { get; set; }
    public string? RequestDataJson { get; set; }
    public DateTime SubmissionDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }

    public List<RequestStatusHistoryDto> StatusHistory { get; set; } = new();
}
