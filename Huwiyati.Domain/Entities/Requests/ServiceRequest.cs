namespace Huwiyati.Domain.Entities.Requests;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Enums;

/// <summary>
/// Represents a formal civil or identity service request submitted by a citizen (e.g., ID renewal, passport issuance).
/// </summary>
public class ServiceRequest : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique public-facing tracking number for the request.
    /// </summary>
    public string RequestNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the citizen submitting or benefiting from the request.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the service type being requested.
    /// </summary>
    public Guid ServiceTypeId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the branch assigned to process the request.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the current processing status of the request.
    /// </summary>
    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    /// <summary>
    /// Gets or sets the stated reason if the request was rejected.
    /// </summary>
    public string? RejectionReason { get; set; }

    /// <summary>
    /// Gets or sets dynamic payload data in JSON format containing service-specific form attributes.
    /// </summary>
    public string? RequestDataJson { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the request was officially submitted.
    /// </summary>
    public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when processing completed (approved, issued, or rejected).
    /// </summary>
    public DateTime? CompletedDate { get; set; }

    /// <summary>
    /// Gets or sets the citizen applicant navigation entity.
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service type navigation entity.
    /// </summary>
    public ServiceType ServiceType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the processing organization branch navigation entity.
    /// </summary>
    public OrganizationBranch Branch { get; set; } = null!;

    /// <summary>
    /// Gets or sets the chronological history of status transitions for this request.
    /// </summary>
    public ICollection<RequestStatusHistory> StatusHistory { get; set; } = new List<RequestStatusHistory>();
}
