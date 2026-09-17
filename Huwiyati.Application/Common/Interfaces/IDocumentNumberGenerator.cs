namespace Huwiyati.Application.Common.Interfaces;

// Interface for generating unique document numbers (National Numbers "01...", Family Numbers "02...")
public interface IDocumentNumberGenerator
{
    Task<string> GenerateNationalNumberAsync(Guid branchId, CancellationToken cancellationToken = default);
    Task<string> GenerateFamilyNumberAsync(Guid branchId, CancellationToken cancellationToken = default);
    Task<string> GenerateBirthCertificateNumberAsync(Guid branchId, CancellationToken cancellationToken = default);
}
