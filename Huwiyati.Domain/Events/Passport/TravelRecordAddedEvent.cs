namespace Huwiyati.Domain.Events.Passport;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a new international travel record is recorded under a passport.
/// </summary>
public class TravelRecordAddedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen traveler.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the passport number used for travel.
    /// </summary>
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the destination or origin country.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of entry into the destination/country.
    /// </summary>
    public DateOnly EntryDate { get; set; }

    /// <summary>
    /// Gets or sets the date of exit from the country, if recorded.
    /// </summary>
    public DateOnly? ExitDate { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TravelRecordAddedEvent"/> class.
    /// </summary>
    /// <param name="personId">The unique identifier of the citizen traveler.</param>
    /// <param name="passportNumber">The passport number.</param>
    /// <param name="country">The country visited or entered.</param>
    /// <param name="entryDate">The date of entry.</param>
    /// <param name="exitDate">The optional date of exit.</param>
    public TravelRecordAddedEvent(Guid personId, string passportNumber, string country, DateOnly entryDate, DateOnly? exitDate)
    {
        PersonId = personId;
        PassportNumber = passportNumber;
        Country = country;
        EntryDate = entryDate;
        ExitDate = exitDate;
    }
}
