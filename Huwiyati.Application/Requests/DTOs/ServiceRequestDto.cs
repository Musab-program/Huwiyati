namespace Huwiyati.Application.Requests.DTOs;

using Huwiyati.Domain.Enums;

/// <summary>
/// Represents data transfer object containing comprehensive details of a civil service request.
/// </summary>
public class ServiceRequestDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the service request.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique public tracking number of the service request.
    /// </summary>
    public string RequestNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the applicant person record.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the applicant person.
    /// </summary>
    public string PersonFullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the national identification number of the applicant.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the requested service type.
    /// </summary>
    public Guid ServiceTypeId { get; set; }

    /// <summary>
    /// Gets or sets the name of the requested service type.
    /// </summary>
    public string ServiceTypeName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the standardized code of the requested service type.
    /// </summary>
    public string ServiceTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the branch assigned to process the request.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the name of the assigned branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current processing status of the service request.
    /// </summary>
    public RequestStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the reason for rejection if the request was rejected.
    /// </summary>
    public string? RejectionReason { get; set; }

    /// <summary>
    /// Gets or sets raw JSON data containing service-specific payload information.
    /// </summary>
    public string? RequestDataJson { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the request was submitted.
    /// </summary>
    public DateTime SubmissionDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the request was completed or fulfilled.
    /// </summary>
    public DateTime? CompletedDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the service request was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the creator username or identifier.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the chronological history of status changes for this request.
    /// </summary>
    public List<RequestStatusHistoryDto> StatusHistory { get; set; } = new();
}
