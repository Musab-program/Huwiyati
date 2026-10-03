namespace Huwiyati.Domain.Events.Passport;

using Huwiyati.Domain.Common;

// Domain Event triggered when a new Passport is issued to a person
public class PassportIssuedEvent : BaseEvent
{
    public Guid PersonId { get; set; }
    public string PassportNumber { get; set; } = string.Empty;
    public DateOnly ExpiryDate { get; set; }

    public PassportIssuedEvent(Guid personId, string passportNumber, DateOnly expiryDate)
    {
        PersonId = personId;
        PassportNumber = passportNumber;
        ExpiryDate = expiryDate;
    }
}
