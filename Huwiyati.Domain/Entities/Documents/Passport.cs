namespace Huwiyati.Domain.Entities.Documents;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Enums;

public class Passport : AuditableEntity
{
    public Guid PersonId { get; set; }
    public Guid IssuingBranchId { get; set; }
    public string PassportNumber { get; set; } = string.Empty;
    public PassportType PassportType { get; set; } = PassportType.Regular;
    public DateOnly IssueDate { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public string? QrCodePayload { get; set; }
    public PassportStatus Status { get; set; } = PassportStatus.Active;

    // Navigation properties
    public Person Person { get; set; } = null!;
    public OrganizationBranch IssuingBranch { get; set; } = null!;
}
