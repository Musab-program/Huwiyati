namespace Huwiyati.Domain.Enums;

/// <summary>
/// Specifies the validity and lifecycle status of a family record and identity card.
/// </summary>
public enum FamilyStatus
{
    /// <summary>
    /// Family record is valid and active.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Family card has surpassed its 10-year statutory validity duration.
    /// </summary>
    Expired = 2
}
