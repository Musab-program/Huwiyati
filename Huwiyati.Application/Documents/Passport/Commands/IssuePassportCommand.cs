namespace Huwiyati.Application.Documents.Passport.Commands;

using Huwiyati.Domain.Enums;

// Command containing details required to issue a new Passport for the first time
public class IssuePassportCommand
{
    // Personal Information
    public string NationalNumber { get; set; } = string.Empty;

    // Optional updated photo URL for the person
    public string? PhotoUrl { get; set; }

    // Passport Specification Information
    public PassportType PassportType { get; set; } = PassportType.Regular;

    // Issuing Branch Information
    public Guid IssuingBranchId { get; set; }
}
