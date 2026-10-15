namespace Huwiyati.Domain.Enums;

/// <summary>
/// Specifies the workflow processing stage of a citizen civil service request.
/// </summary>
public enum RequestStatus
{
    /// <summary>
    /// Submitted by citizen applicant and queued for branch processing.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Assigned to and actively under review by an authorized branch employee.
    /// </summary>
    UnderReview = 2,

    /// <summary>
    /// Verified and approved by the branch employee, ready for document fulfillment.
    /// </summary>
    Approved = 3,

    /// <summary>
    /// Rejected by employee with a mandatory documented rejection reason.
    /// </summary>
    Rejected = 4,

    /// <summary>
    /// Officially approved, fulfilled, and the resultant official document issued.
    /// </summary>
    Issued = 5,

    /// <summary>
    /// Voluntarily cancelled by the citizen applicant prior to finalization.
    /// </summary>
    Cancelled = 6
}
