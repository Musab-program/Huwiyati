namespace Huwiyati.Domain.Entities.Authentication;

using Huwiyati.Domain.Common;

/// <summary>
/// Represents a cryptographically secure refresh token used to extend authenticated user sessions.
/// Manages rotation lifecycle and token revocation according to OAuth2 standards.
/// </summary>
public class RefreshToken : BaseEntity
{
    /// <summary>
    /// Gets or sets the foreign key identifier of the associated user account.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique cryptographically random token string.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier (JTI) of the paired JWT access token.
    /// </summary>
    public string JwtId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this refresh token has already been exchanged for a new pair.
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this token was explicitly invalidated or revoked.
    /// </summary>
    public bool IsRevoked { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the token was originally generated.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the UTC timestamp when the refresh token expires.
    /// </summary>
    public DateTime ExpiryDate { get; set; }
}
