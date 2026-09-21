namespace Huwiyati.Domain.Entities.Documents;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;

public class DeathCertificate : AuditableEntity
{
    public Guid PersonId { get; set; }
    public Guid HospitalBranchId { get; set; }
    public Guid IssuingBranchId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public DateOnly DeathDate { get; set; }
    public string? PlaceOfDeath { get; set; }
    public string? CauseOfDeath { get; set; }
    public DateOnly IssueDate { get; set; }

    // Navigation Properties
    public Person Person { get; set; } = null!;
    public OrganizationBranch HospitalBranch { get; set; } = null!;
    public OrganizationBranch IssuingBranch { get; set; } = null!;
}
