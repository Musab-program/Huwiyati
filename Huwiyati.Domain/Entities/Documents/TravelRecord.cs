namespace Huwiyati.Domain.Entities.Documents;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.Organizations;

public class TravelRecord : AuditableEntity
{
    public Guid PassportId { get; set; }
    public Guid IssuingBranchId { get; set; }
    public string Country { get; set; } = string.Empty;
    public DateOnly EntryDate { get; set; }
    public DateOnly? ExitDate { get; set; }

    // Navigation properties
    public Passport Passport { get; set; } = null!;
    public OrganizationBranch IssuingBranch { get; set; } = null!;
}
