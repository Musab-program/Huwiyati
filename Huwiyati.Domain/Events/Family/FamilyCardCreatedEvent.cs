namespace Huwiyati.Domain.Events.Family;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a new family record and family card have been registered.
/// </summary>
public class FamilyCardCreatedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen designated as head of family.
    /// </summary>
    public Guid HeadOfFamilyPersonId { get; set; }

    /// <summary>
    /// Gets or sets the newly assigned family registry number.
    /// </summary>
    public string FamilyNumber { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="FamilyCardCreatedEvent"/> class.
    /// </summary>
    /// <param name="headOfFamilyPersonId">The unique identifier of the head of family.</param>
    /// <param name="familyNumber">The family registry card number.</param>
    public FamilyCardCreatedEvent(Guid headOfFamilyPersonId, string familyNumber)
    {
        HeadOfFamilyPersonId = headOfFamilyPersonId;
        FamilyNumber = familyNumber;
    }
}
