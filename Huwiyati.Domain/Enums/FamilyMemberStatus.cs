namespace Huwiyati.Domain.Enums;

/// <summary>
/// Specifies the operational and legal status of an individual member within a registered family record.
/// </summary>
public enum FamilyMemberStatus
{
    /// <summary>
    /// Member is an active, recognized constituent of the family card.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Member was divorced and legally detached from the family unit.
    /// </summary>
    Divorced = 2,

    /// <summary>
    /// Member is deceased with certified death registration.
    /// </summary>
    Deceased = 3,

    /// <summary>
    /// Member is legally separated or moved into a distinct independent family record.
    /// </summary>
    Separated = 4
}
