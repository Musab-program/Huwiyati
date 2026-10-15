namespace Huwiyati.Domain.Enums;

/// <summary>
/// Specifies the legal and physical validity status of a citizen's national identification card.
/// </summary>
public enum NationalIdCardStatus
{
    /// <summary>
    /// Identification card is active and legally recognized.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Identification card has expired past its 10-year expiration date.
    /// </summary>
    Expired = 2
}
