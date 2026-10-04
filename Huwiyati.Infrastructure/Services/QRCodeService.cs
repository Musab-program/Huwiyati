namespace Huwiyati.Infrastructure.Services;

using System.Security.Cryptography;
using Huwiyati.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Infrastructure implementation of IQRCodeService using cryptographically secure random number generation.
/// </summary>
public class QRCodeService : IQRCodeService
{
    private readonly string _baseUrl;

    public QRCodeService(IConfiguration configuration)
    {
        // Step 1: Retrieve the application base URL from configuration or fallback to default
        _baseUrl = configuration["AppSetting:BaseUrl"] ?? "https://hawiyati.example";
    }

    /// <summary>
    /// Step 2: Generate 32 cryptographically random bytes (256 bits entropy) and format as URL-safe Base64
    /// </summary>
    public string GenerateVerificationToken()
    {
        var randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        // Convert to URL-safe string by replacing +, / and trimming =
        return Convert.ToBase64String(randomBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }

    /// <summary>
    /// Step 3: Construct the final verification URL by combining base URL and payload token
    /// </summary>
    public string BuildVerificationUrl(string token)
    {
        return $"{_baseUrl.TrimEnd('/')}/verify/{token}";
    }
}
