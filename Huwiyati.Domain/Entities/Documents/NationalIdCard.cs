using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Enums;

namespace Huwiyati.Domain.Entities.Documents;

public class NationalIdCard : AuditableEntity
{
    public Guid PersonId { get; set; }
    public Guid IssuingBranchId { get; set; }
    public DateOnly IssueDate { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public string? QrCodePayload { get; set; } = string.Empty;
    public NationalIdCardStatus Status { get; set; } = NationalIdCardStatus.Active;

    //Naviagation property
    public Person Person { get; set; } = null!;
    public OrganizationBranch OrganizationBranch { get; set; } = null!;

}
