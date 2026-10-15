namespace Huwiyati.Domain.Enums;

/// <summary>
/// Specifies the vitality and legal status of an individual person in the national civil registry.
/// </summary>
public enum PersonStatus
{
    /// <summary>
    /// Person is living, in good legal standing, and eligible for civil services.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Civil record is temporarily suspended pending legal or verification inquiry.
    /// </summary>
    Suspended = 2,

    /// <summary>
    /// Person is legally certified as deceased with an associated death certificate.
    /// </summary>
    Deceased = 3
}
