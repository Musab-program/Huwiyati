namespace Huwiyati.Domain.Events.Family;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an existing family card has been successfully renewed.
/// </summary>
public class FamilyCardRenewedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen designated as head of family.
    /// </summary>
    public Guid HeadOfFamilyPersonId { get; set; }

    /// <summary>
    /// Gets or sets the renewed family registry card number.
    /// </summary>
    public string FamilyNumber { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="FamilyCardRenewedEvent"/> class.
    /// </summary>
    /// <param name="headOfFamilyPersonId">The unique identifier of the head of family.</param>
    /// <param name="familyNumber">The renewed family card number.</param>
    public FamilyCardRenewedEvent(Guid headOfFamilyPersonId, string familyNumber)
    {
        HeadOfFamilyPersonId = headOfFamilyPersonId;
        FamilyNumber = familyNumber;
    }
}
