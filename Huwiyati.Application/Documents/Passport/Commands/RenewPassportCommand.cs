namespace Huwiyati.Application.Documents.Passport.Commands;

using Huwiyati.Domain.Enums;

// Command containing required details to renew an existing Passport
public class RenewPassportCommand
{
    // Identification
    public string NationalNumber { get; set; } = string.Empty;

    // Renewing Branch Information
    public Guid IssuingBranchId { get; set; }

    // Optional updated photo URL and passport type
    public string? PhotoUrl { get; set; }
    public PassportType? PassportType { get; set; }
}
