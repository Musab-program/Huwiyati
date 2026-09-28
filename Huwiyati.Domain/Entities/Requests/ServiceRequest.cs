namespace Huwiyati.Domain.Entities.Requests;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Enums;

public class ServiceRequest : AuditableEntity
{
    public string RequestNumber { get; set; } = string.Empty;
    public Guid PersonId { get; set; }
    public Guid ServiceTypeId { get; set; }
    public Guid BranchId { get; set; }
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public string? RejectionReason { get; set; }
    public string? RequestDataJson { get; set; }
    public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedDate { get; set; }

    // Navigation Properties
    public Person Person { get; set; } = null!;
    public ServiceType ServiceType { get; set; } = null!;
    public OrganizationBranch Branch { get; set; } = null!;
    public ICollection<RequestStatusHistory> StatusHistory { get; set; } = new List<RequestStatusHistory>();
}
