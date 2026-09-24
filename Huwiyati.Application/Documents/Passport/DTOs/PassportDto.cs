namespace Huwiyati.Application.Documents.Passport.DTOs;

using Huwiyati.Domain.Enums;

// DTO representing Passport details
public class PassportDto
{
    // Passport Identifier
    public Guid Id { get; set; }

    // Personal Information
    public Guid PersonId { get; set; }
    public string PersonFullName { get; set; } = string.Empty;
    public string NationalNumber { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }

    // Passport Information
    public string PassportNumber { get; set; } = string.Empty;
    public PassportType PassportType { get; set; }
    public DateOnly IssueDate { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public string? QrCodePayload { get; set; }
    public PassportStatus Status { get; set; }

    // Issuing Branch Information
    public Guid IssuingBranchId { get; set; }
    public string IssuingBranchName { get; set; } = string.Empty;

    // Audit Information
    public DateTime CreatedAt { get; set; }
}
