namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a person's civil registry profile or biographical data has been updated.
/// </summary>
public class PersonDataUpdatedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the person whose record was updated.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonDataUpdatedEvent"/> class.
    /// </summary>
    /// <param name="personId">The unique identifier of the person.</param>
    public PersonDataUpdatedEvent(Guid personId)
    {
        PersonId = personId;
    }
}
