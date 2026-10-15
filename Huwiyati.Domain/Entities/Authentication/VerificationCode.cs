namespace Huwiyati.Domain.Entities.Authentication;

using Huwiyati.Domain.Common;

/// <summary>
/// Represents a time-limited one-time password (OTP) verification code issued for authentication, password reset, or device registration.
/// Tracks expiration, verification attempts, and single-use redemption status.
/// </summary>
public class VerificationCode : BaseEntity
{
    /// <summary>
    /// Gets or sets the foreign key identifier of the user account to which this code was issued.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the numerical or alphanumeric one-time verification code string.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC timestamp when this verification code expires and becomes invalid.
    /// </summary>
    public DateTime ExpirationTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this code has already been successfully consumed.
    /// </summary>
    public bool IsUsed { get; set; } = false;

    /// <summary>
    /// Gets or sets the count of failed verification attempts to guard against brute-force attacks.
    /// </summary>
    public int Attempts { get; set; } = 0;

    /// <summary>
    /// Gets or sets the UTC timestamp when the code was generated and dispatched.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
