namespace Huwiyati.Domain.Entities.Documents;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;

/// <summary>
/// Represents an official death certificate issued for a deceased citizen.
/// </summary>
public class DeathCertificate : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the deceased person.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the hospital or medical facility where death was certified.
    /// </summary>
    public Guid HospitalBranchId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the civil registry branch issuing the certificate.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the unique certificate serial number.
    /// </summary>
    public string CertificateNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the recorded date of death.
    /// </summary>
    public DateOnly DeathDate { get; set; }

    /// <summary>
    /// Gets or sets the geographical place or facility where death occurred.
    /// </summary>
    public string? PlaceOfDeath { get; set; }

    /// <summary>
    /// Gets or sets the certified medical cause of death.
    /// </summary>
    public string? CauseOfDeath { get; set; }

    /// <summary>
    /// Gets or sets the date the certificate was officially issued.
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the encrypted or structured QR code payload for verification purposes.
    /// </summary>
    public string? QrCodePayload { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the deceased person navigation entity.
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// Gets or sets the certifying hospital or medical facility branch navigation entity.
    /// </summary>
    public OrganizationBranch HospitalBranch { get; set; } = null!;

    /// <summary>
    /// Gets or sets the civil registry issuing branch navigation entity.
    /// </summary>
    public OrganizationBranch IssuingBranch { get; set; } = null!;
}
