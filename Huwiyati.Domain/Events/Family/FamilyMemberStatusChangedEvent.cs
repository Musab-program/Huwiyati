namespace Huwiyati.Domain.Events.Family;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a family member's status (such as separation, death, or marriage) changes.
/// </summary>
public class FamilyMemberStatusChangedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen designated as head of family.
    /// </summary>
    public Guid HeadOfFamilyPersonId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the family member whose status changed.
    /// </summary>
    public string MemberName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="FamilyMemberStatusChangedEvent"/> class.
    /// </summary>
    /// <param name="headOfFamilyPersonId">The unique identifier of the head of family.</param>
    /// <param name="memberName">The full name of the family member.</param>
    public FamilyMemberStatusChangedEvent(Guid headOfFamilyPersonId, string memberName)
    {
        HeadOfFamilyPersonId = headOfFamilyPersonId;
        MemberName = memberName;
    }
}
