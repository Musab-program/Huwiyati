namespace Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Defines generator operations for producing unique tracking numbers for citizen service requests.
/// </summary>
public interface IRequestNumberGenerator
{
    /// <summary>
    /// Generates a unique tracking number for a newly initiated service request.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A unique request tracking number string.</returns>
    Task<string> GenerateRequestNumberAsync(CancellationToken cancellationToken = default);
}
