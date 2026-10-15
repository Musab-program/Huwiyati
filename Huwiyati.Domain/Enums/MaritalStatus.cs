namespace Huwiyati.Domain.Enums;

/// <summary>
/// Specifies the legal marital status of a person in civil records.
/// </summary>
public enum MaritalStatus
{
    /// <summary>
    /// Individual has never been married.
    /// </summary>
    Single = 1,

    /// <summary>
    /// Individual is currently legally married.
    /// </summary>
    Married = 2,

    /// <summary>
    /// Individual was legally divorced and not currently remarried.
    /// </summary>
    Divorced = 3,

    /// <summary>
    /// Individual's spouse is deceased and they have not remarried.
    /// </summary>
    Widowed = 4
}
