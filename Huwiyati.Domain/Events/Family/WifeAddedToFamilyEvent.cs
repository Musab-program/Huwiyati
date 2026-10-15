namespace Huwiyati.Domain.Events.Family;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a wife is officially registered and added to a family record.
/// </summary>
public class WifeAddedToFamilyEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen designated as head of family (husband).
    /// </summary>
    public Guid HeadOfFamilyPersonId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the wife person.
    /// </summary>
    public Guid WifePersonId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the wife.
    /// </summary>
    public string WifeName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="WifeAddedToFamilyEvent"/> class.
    /// </summary>
    /// <param name="headOfFamilyPersonId">The unique identifier of the head of family.</param>
    /// <param name="wifePersonId">The unique identifier of the wife.</param>
    /// <param name="wifeName">The full name of the wife.</param>
    public WifeAddedToFamilyEvent(Guid headOfFamilyPersonId, Guid wifePersonId, string wifeName)
    {
        HeadOfFamilyPersonId = headOfFamilyPersonId;
        WifePersonId = wifePersonId;
        WifeName = wifeName;
    }
}
