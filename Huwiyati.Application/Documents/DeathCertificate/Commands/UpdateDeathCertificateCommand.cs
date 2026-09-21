namespace Huwiyati.Application.Documents.DeathCertificate.Commands;

public class UpdateDeathCertificateCommand
{
    public Guid DeathCertificateId { get; set; }
    public DateOnly DeathDate { get; set; }
    public string PlaceOfDeath { get; set; } = string.Empty;
    public string CauseOfDeath { get; set; } = string.Empty;
}
