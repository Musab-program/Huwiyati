namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a child's civil registry records have been updated.
/// </summary>
public class ChildDataUpdatedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the father.
    /// </summary>
    public Guid FatherPersonId { get; set; }

    /// <summary>
    /// Gets or sets the updated full name of the child.
    /// </summary>
    public string ChildName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChildDataUpdatedEvent"/> class.
    /// </summary>
    /// <param name="fatherPersonId">The unique identifier of the father.</param>
    /// <param name="childName">The updated full name of the child.</param>
    public ChildDataUpdatedEvent(Guid fatherPersonId, string childName)
    {
        FatherPersonId = fatherPersonId;
        ChildName = childName;
    }
}
