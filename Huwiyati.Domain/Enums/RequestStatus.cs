namespace Huwiyati.Domain.Enums;

public enum RequestStatus
{
    Pending = 1,      // Submitted by applicant, waiting in branch queue
    UnderReview = 2,  // Being reviewed by branch employee
    Approved = 3,     // Approved by employee
    Rejected = 4,     // Rejected by employee with a reason
    Issued = 5,       // Official document generated and issued
    Cancelled = 6     // Cancelled by applicant/submitter
}
