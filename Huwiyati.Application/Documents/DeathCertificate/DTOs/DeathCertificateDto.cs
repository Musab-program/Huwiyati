namespace Huwiyati.Application.Documents.DeathCertificate.DTOs;

public class DeathCertificateDto
{
    public Guid Id { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }

    // Deceased Person Details
    public Guid PersonId { get; set; }
    public string NationalNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;

    // Death Details
    public DateOnly DeathDate { get; set; }
    public string? PlaceOfDeath { get; set; }
    public string? CauseOfDeath { get; set; }

    // Hospital Details
    public Guid HospitalBranchId { get; set; }
    public string HospitalName { get; set; } = string.Empty;

    // Civil Registry Issuing Branch Details
    public Guid IssuingBranchId { get; set; }
    public string IssuingBranchName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
