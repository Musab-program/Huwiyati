namespace Huwiyati.Application.Documents.NationalIdCard.DTOs;

// Data Transfer Object representing the issued National ID Card details returned to client
public class NationalIdCardDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string NationalNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Guid IssuingBranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public string? QrCodePayload { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
