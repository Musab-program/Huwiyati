namespace Huwiyati.Domain.Entities.Documents;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;

/// <summary>
/// Represents an official birth certificate issued for a newborn or citizen.
/// </summary>
public class BirthCertificate : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the registered child.
    /// </summary>
    public Guid ChildPersonId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the father.
    /// </summary>
    public Guid FatherPersonId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the mother.
    /// </summary>
    public Guid MotherPersonId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the hospital or health facility branch where birth was recorded.
    /// </summary>
    public Guid HospitalBranchId { get; set; }

    /// <summary>
    /// Gets or sets the unique certificate serial number.
    /// </summary>
    public string CertificateNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the official issue date of the certificate.
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the encrypted or structured QR code payload for verification purposes.
    /// </summary>
    public string? QrCodePayload { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the registered child navigation entity.
    /// </summary>
    public Person ChildPerson { get; set; } = null!;

    /// <summary>
    /// Gets or sets the father navigation entity.
    /// </summary>
    public Person FatherPerson { get; set; } = null!;

    /// <summary>
    /// Gets or sets the mother navigation entity.
    /// </summary>
    public Person MotherPerson { get; set; } = null!;

    /// <summary>
    /// Gets or sets the hospital or issuing branch navigation entity.
    /// </summary>
    public OrganizationBranch HospitalBranch { get; set; } = null!;
}
