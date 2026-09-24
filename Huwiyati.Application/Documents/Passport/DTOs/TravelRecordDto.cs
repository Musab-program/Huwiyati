namespace Huwiyati.Application.Documents.Passport.DTOs;

// DTO representing Travel Record details
public class TravelRecordDto
{
    public Guid Id { get; set; }

    // Passport Information
    public Guid PassportId { get; set; }
    public string PassportNumber { get; set; } = string.Empty;

    // Person Information
    public Guid PersonId { get; set; }
    public string PersonFullName { get; set; } = string.Empty;
    public string NationalNumber { get; set; } = string.Empty;

    // Issuing Branch / Border Port Information
    public Guid IssuingBranchId { get; set; }
    public string IssuingBranchName { get; set; } = string.Empty;

    // Travel Movement Details
    public string Country { get; set; } = string.Empty;
    public DateOnly EntryDate { get; set; }
    public DateOnly? ExitDate { get; set; }

    // Audit Information
    public DateTime CreatedAt { get; set; }
}
