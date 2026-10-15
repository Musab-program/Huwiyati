namespace Huwiyati.Application.Documents.BirthCertificate.DTOs;

/// <summary>
/// Represents data transfer object for birth certificate details.
/// </summary>
public class BirthCertificateDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the birth certificate.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the official certificate number.
    /// </summary>
    public string CertificateNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date on which the certificate was issued.
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the encrypted or structured payload encoded in the certificate QR code.
    /// </summary>
    public string? QrCodePayload { get; set; }

    /// <summary>
    /// Gets or sets the unique person identifier of the newborn child.
    /// </summary>
    public Guid ChildPersonId { get; set; }

    /// <summary>
    /// Gets or sets the national number assigned to the newborn child.
    /// </summary>
    public string ChildNationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the newborn child.
    /// </summary>
    public string ChildFullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gender of the child.
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of birth of the child.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the place of birth of the child.
    /// </summary>
    public string PlaceOfBirth { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique person identifier of the father.
    /// </summary>
    public Guid FatherPersonId { get; set; }

    /// <summary>
    /// Gets or sets the national number of the father.
    /// </summary>
    public string FatherNationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the father.
    /// </summary>
    public string FatherFullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique person identifier of the mother.
    /// </summary>
    public Guid MotherPersonId { get; set; }

    /// <summary>
    /// Gets or sets the national number of the mother.
    /// </summary>
    public string MotherNationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the mother.
    /// </summary>
    public string MotherFullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch identifier of the issuing hospital.
    /// </summary>
    public Guid HospitalBranchId { get; set; }

    /// <summary>
    /// Gets or sets the name of the issuing hospital.
    /// </summary>
    public string HospitalName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the family identifier to which the child belongs.
    /// </summary>
    public Guid FamilyId { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the certificate record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
