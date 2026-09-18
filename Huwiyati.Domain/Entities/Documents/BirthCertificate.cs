namespace Huwiyati.Domain.Entities.Documents;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;

public class BirthCertificate : AuditableEntity
{
    public Guid ChildPersonId { get; set; }
    public Guid FatherPersonId { get; set; }
    public Guid MotherPersonId { get; set; }
    public Guid HospitalBranchId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }

    // Navigation Properties
    public Person ChildPerson { get; set; } = null!;
    public Person FatherPerson { get; set; } = null!;
    public Person MotherPerson { get; set; } = null!;
    public OrganizationBranch HospitalBranch { get; set; } = null!;
}
