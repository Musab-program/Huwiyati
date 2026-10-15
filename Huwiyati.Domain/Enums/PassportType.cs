namespace Huwiyati.Domain.Enums;

/// <summary>
/// Specifies the official category of passport issued according to diplomatic and civil statutes.
/// </summary>
public enum PassportType
{
    /// <summary>
    /// Standard ordinary citizen international travel passport.
    /// </summary>
    Regular = 1,

    /// <summary>
    /// Diplomatic passport issued to sovereign diplomats and accredited officials.
    /// </summary>
    Diplomatic = 2,

    /// <summary>
    /// Official special passport issued for government business and state missions.
    /// </summary>
    Special = 3
}
