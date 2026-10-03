namespace Huwiyati.Domain.Events.Passport;

using Huwiyati.Domain.Common;

// Domain Event triggered when an existing Passport is renewed for a person
public class PassportRenewedEvent : BaseEvent
{
    public Guid PersonId { get; set; }
    public string PassportNumber { get; set; } = string.Empty;
    public DateOnly NewExpiryDate { get; set; }

    public PassportRenewedEvent(Guid personId, string passportNumber, DateOnly newExpiryDate)
    {
        PersonId = personId;
        PassportNumber = passportNumber;
        NewExpiryDate = newExpiryDate;
    }
}
