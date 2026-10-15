namespace Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Defines generator operations for producing unique sequential document identification numbers across all civil and passport modules.
/// </summary>
public interface IDocumentNumberGenerator
{
    /// <summary>
    /// Generates a unique national identification number (format "01..." followed by year, branch, and sequence).
    /// </summary>
    /// <param name="branchId">The identifier of the issuing branch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A unique national number string.</returns>
    Task<string> GenerateNationalNumberAsync(Guid branchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a unique family book number (format "02..." followed by year, branch, and sequence).
    /// </summary>
    /// <param name="branchId">The identifier of the issuing branch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A unique family number string.</returns>
    Task<string> GenerateFamilyNumberAsync(Guid branchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a unique birth certificate registration number.
    /// </summary>
    /// <param name="branchId">The identifier of the recording hospital or registry branch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A unique birth certificate number string.</returns>
    Task<string> GenerateBirthCertificateNumberAsync(Guid branchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a unique death certificate registration number.
    /// </summary>
    /// <param name="branchId">The identifier of the issuing registry branch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A unique death certificate number string.</returns>
    Task<string> GenerateDeathCertificateNumberAsync(Guid branchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a unique passport travel document number.
    /// </summary>
    /// <param name="branchId">The identifier of the issuing passport branch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A unique passport number string.</returns>
    Task<string> GeneratePassportNumberAsync(Guid branchId, CancellationToken cancellationToken = default);
}
