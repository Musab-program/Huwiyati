namespace Huwiyati.Application.Documents.BirthCertificate.Commands;

using Huwiyati.Domain.Enums;

/// <summary>
/// Command payload for registering a newborn child and issuing a birth certificate.
/// </summary>
public class IssueBirthCertificateCommand
{
    /// <summary>
    /// Gets or sets the national number of the child's father.
    /// </summary>
    public string FatherNationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the national number of the child's mother.
    /// </summary>
    public string MotherNationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first name of the newborn child.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of birth of the newborn child.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the place or city where the child was born.
    /// </summary>
    public string PlaceOfBirth { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gender of the newborn child.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Gets or sets the blood group of the newborn child.
    /// </summary>
    public BloodGroup BloodGroup { get; set; }

    /// <summary>
    /// Gets or sets the governorate of residence or birth.
    /// </summary>
    public string Governorate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the district of residence or birth.
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets detailed address information for the residence.
    /// </summary>
    public string AddressDetails { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch identifier of the hospital where the birth took place.
    /// </summary>
    public Guid HospitalBranchId { get; set; }

    /// <summary>
    /// Gets or sets the family identifier to which the child is being registered.
    /// </summary>
    public Guid FamilyId { get; set; }

    /// <summary>
    /// Gets or sets the civil registry branch identifier responsible for issuing the certificate.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the optional explicit date of issue; defaults to current date if omitted.
    /// </summary>
    public DateOnly? IssueDate { get; set; }
}
