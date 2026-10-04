namespace Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Interface for QR Code token generation and verification URL building.
/// </summary>
public interface IQRCodeService
{
    /// <summary>
    /// Step 1: Generates a high-entropy cryptographically secure random verification token.
    /// </summary>
    /// <returns>A URL-safe Base64 random string.</returns>
    string GenerateVerificationToken();

    /// <summary>
    /// Step 2: Builds the complete verification URL from a given token string.
    /// </summary>
    /// <param name="token">The verification token or QR payload.</param>
    /// <returns>Full verification URL string.</returns>
    string BuildVerificationUrl(string token);
}
