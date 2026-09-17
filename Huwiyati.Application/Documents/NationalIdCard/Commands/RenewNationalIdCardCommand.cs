namespace Huwiyati.Application.Documents.NationalIdCard.Commands;

using Huwiyati.Domain.Enums;

// Command model for renewing an existing National ID Card (IssueDate is fixed to UTC today automatically)
public class RenewNationalIdCardCommand
{
    // Unique 11-digit National Number identifying the citizen
    public string NationalNumber { get; set; } = string.Empty;

    // Optional updatable Person fields during renewal
    public MaritalStatus? MaritalStatus { get; set; }
    public string? Governorate { get; set; }
    public string? District { get; set; }
    public string? AddressDetails { get; set; }
    public string? PhotoUrl { get; set; }

    // Issuing Branch for the new card
    public Guid IssuingBranchId { get; set; }
}
