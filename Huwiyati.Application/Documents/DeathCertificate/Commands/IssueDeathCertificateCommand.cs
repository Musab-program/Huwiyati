namespace Huwiyati.Application.Documents.DeathCertificate.Commands;

public class IssueDeathCertificateCommand
{
    // National Number for Deceased Person
    public string NationalNumber { get; set; } = string.Empty;

    // Organization & Branch References
    public Guid HospitalBranchId { get; set; }
    public Guid IssuingBranchId { get; set; }

    // Death Details
    public DateOnly DeathDate { get; set; }
    public string? PlaceOfDeath { get; set; }
    public string? CauseOfDeath { get; set; }
}
