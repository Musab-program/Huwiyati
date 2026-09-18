namespace Huwiyati.Application.Documents.BirthCertificate.Commands;

using Huwiyati.Domain.Enums;

public class IssueBirthCertificateCommand
{
    // National Numbers for Parents
    public string FatherNationalNumber { get; set; } = string.Empty;
    public string MotherNationalNumber { get; set; } = string.Empty;

    // Newborn Child Details
    public string FirstName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string PlaceOfBirth { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public BloodGroup BloodGroup { get; set; }
    public string Governorate { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string AddressDetails { get; set; } = string.Empty;

    // Organization & Branch References
    public Guid HospitalBranchId { get; set; }
    public Guid FamilyId { get; set; }
    public Guid IssuingBranchId { get; set; }
    public DateOnly? IssueDate { get; set; }
}
