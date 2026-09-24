namespace Huwiyati.Application.Documents.Passport.Commands;

// Command containing details required to add a new travel record
public class AddTravelRecordCommand
{
    // Printed Passport Number
    public string PassportNumber { get; set; } = string.Empty;

    // Border Branch / Port Information
    public Guid IssuingBranchId { get; set; }

    // Destination / Country Information
    public string Country { get; set; } = string.Empty;

    // Travel Dates
    public DateOnly EntryDate { get; set; }
    public DateOnly? ExitDate { get; set; }
}
