namespace Huwiyati.Application.Documents.Verification.DTOs;

/// <summary>
/// Data Transfer Object returning minimal necessary identity data for document verifiers (Selective Disclosure).
/// </summary>
public class DocumentVerificationResultDto
{
    /// <summary>
    /// Gets or sets a value indicating whether the verified document is valid and untampered.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the document type (e.g. NationalIdCard, Passport, BirthCertificate, DeathCertificate).
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the official document number.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full legal name of the document holder.
    /// </summary>
    public string HolderName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the biometric photo URL of the holder, if applicable.
    /// </summary>
    public string? HolderPhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the current status of the document.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of issue.
    /// </summary>
    public DateOnly? IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the date of expiration, if applicable.
    /// </summary>
    public DateOnly? ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the issuing branch name.
    /// </summary>
    public string? IssuingBranchName { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the document verification was completed.
    /// </summary>
    public DateTime VerificationTimestamp { get; set; } = DateTime.UtcNow;
}
