namespace Huwiyati.Application.Documents.DeathCertificate.DTOs;

/// <summary>
/// Represents data transfer object for death certificate records.
/// </summary>
public class DeathCertificateDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the death certificate.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the official certificate number.
    /// </summary>
    public string CertificateNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the issuance date of the death certificate.
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the encrypted QR code verification payload.
    /// </summary>
    public string? QrCodePayload { get; set; }

    /// <summary>
    /// Gets or sets the person identifier of the deceased citizen.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the national number of the deceased citizen.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the deceased citizen.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of birth of the deceased.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the gender of the deceased.
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the recorded date of death.
    /// </summary>
    public DateOnly DeathDate { get; set; }

    /// <summary>
    /// Gets or sets the geographic location or facility where death occurred.
    /// </summary>
    public string? PlaceOfDeath { get; set; }

    /// <summary>
    /// Gets or sets the medical or reported cause of death.
    /// </summary>
    public string? CauseOfDeath { get; set; }

    /// <summary>
    /// Gets or sets the branch identifier of the hospital reporting the death.
    /// </summary>
    public Guid HospitalBranchId { get; set; }

    /// <summary>
    /// Gets or sets the name of the hospital reporting the death.
    /// </summary>
    public string HospitalName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch identifier of the civil registry that issued the certificate.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the name of the civil registry issuing branch.
    /// </summary>
    public string IssuingBranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the certificate was recorded in the system.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
