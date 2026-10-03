namespace Huwiyati.Domain.Events.Passport;

using Huwiyati.Domain.Common;

// Domain Event triggered when a new Travel Record is added to a Passport
public class TravelRecordAddedEvent : BaseEvent
{
    public Guid PersonId { get; set; }
    public string PassportNumber { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateOnly EntryDate { get; set; }
    public DateOnly? ExitDate { get; set; }

    public TravelRecordAddedEvent(Guid personId, string passportNumber, string country, DateOnly entryDate, DateOnly? exitDate)
    {
        PersonId = personId;
        PassportNumber = passportNumber;
        Country = country;
        EntryDate = entryDate;
        ExitDate = exitDate;
    }
}
