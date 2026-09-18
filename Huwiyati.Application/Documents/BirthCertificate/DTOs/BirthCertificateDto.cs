namespace Huwiyati.Application.Documents.BirthCertificate.DTOs;

public class BirthCertificateDto
{
    public Guid Id { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }

    // Newborn Child Details
    public Guid ChildPersonId { get; set; }
    public string ChildNationalNumber { get; set; } = string.Empty;
    public string ChildFullName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string PlaceOfBirth { get; set; } = string.Empty;

    // Parent Details
    public Guid FatherPersonId { get; set; }
    public string FatherNationalNumber { get; set; } = string.Empty;
    public string FatherFullName { get; set; } = string.Empty;

    public Guid MotherPersonId { get; set; }
    public string MotherNationalNumber { get; set; } = string.Empty;
    public string MotherFullName { get; set; } = string.Empty;

    // Organization & Branch References
    public Guid HospitalBranchId { get; set; }
    public string HospitalName { get; set; } = string.Empty;
    public Guid FamilyId { get; set; }
    public DateTime CreatedAt { get; set; }
}
