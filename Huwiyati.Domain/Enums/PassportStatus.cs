namespace Huwiyati.Domain.Enums;

/// <summary>
/// Specifies the legal travel and validity status of a sovereign passport document.
/// </summary>
public enum PassportStatus
{
    /// <summary>
    /// Passport is active and valid for international travel.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Passport has reached its 6-year validity expiration date.
    /// </summary>
    Expired = 2,

    /// <summary>
    /// Passport was revoked or cancelled by immigration authorities.
    /// </summary>
    Canceled = 3
}
