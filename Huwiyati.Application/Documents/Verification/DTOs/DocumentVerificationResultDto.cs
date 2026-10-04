namespace Huwiyati.Application.Documents.Verification.DTOs;

/// <summary>
/// Data Transfer Object returning minimal necessary identity data for document verifiers (Selective Disclosure).
/// </summary>
public class DocumentVerificationResultDto
{
    // Step 1: Verification status flag
    public bool IsValid { get; set; }

    // Step 2: Document identification properties
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;

    // Step 3: Citizen identity details (Holder photo is nullable)
    public string HolderName { get; set; } = string.Empty;
    public string? HolderPhotoUrl { get; set; }

    // Step 4: Document lifecycle dates and status
    public string Status { get; set; } = string.Empty;
    public DateOnly? IssueDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? IssuingBranchName { get; set; }

    // Step 5: Verification audit timestamp
    public DateTime VerificationTimestamp { get; set; } = DateTime.UtcNow;
}
