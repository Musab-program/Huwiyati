namespace Huwiyati.Application.Documents.DeathCertificate.Commands;

/// <summary>
/// Command payload for updating details of an existing death certificate.
/// </summary>
public class UpdateDeathCertificateCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the death certificate to update.
    /// </summary>
    public Guid DeathCertificateId { get; set; }

    /// <summary>
    /// Gets or sets the corrected or updated date of death.
    /// </summary>
    public DateOnly DeathDate { get; set; }

    /// <summary>
    /// Gets or sets the corrected or updated place of death.
    /// </summary>
    public string PlaceOfDeath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the corrected or updated cause of death.
    /// </summary>
    public string CauseOfDeath { get; set; } = string.Empty;
}
