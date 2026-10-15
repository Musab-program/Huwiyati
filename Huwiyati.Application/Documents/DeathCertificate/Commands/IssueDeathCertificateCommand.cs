namespace Huwiyati.Application.Documents.DeathCertificate.Commands;

/// <summary>
/// Command payload for recording a death and issuing an official death certificate.
/// </summary>
public class IssueDeathCertificateCommand
{
    /// <summary>
    /// Gets or sets the national number of the deceased person.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hospital branch identifier reporting or confirming the death.
    /// </summary>
    public Guid HospitalBranchId { get; set; }

    /// <summary>
    /// Gets or sets the civil registry branch identifier issuing the death certificate.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the date on which the person died.
    /// </summary>
    public DateOnly DeathDate { get; set; }

    /// <summary>
    /// Gets or sets the location or facility where death occurred.
    /// </summary>
    public string? PlaceOfDeath { get; set; }

    /// <summary>
    /// Gets or sets the declared or medical cause of death.
    /// </summary>
    public string? CauseOfDeath { get; set; }
}
