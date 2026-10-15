namespace Huwiyati.Application.Documents.Passport.DTOs;

using Huwiyati.Domain.Enums;

/// <summary>
/// Represents data transfer object containing passport issuance and citizen details.
/// </summary>
public class PassportDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the passport record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique person identifier of the passport holder.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the full legal name of the passport holder.
    /// </summary>
    public string PersonFullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the national number of the passport holder.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the biometric photo URL of the passport holder.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the official passport number.
    /// </summary>
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category/type of the passport (e.g. Regular, Diplomatic).
    /// </summary>
    public PassportType PassportType { get; set; }

    /// <summary>
    /// Gets or sets the official date of issue.
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the passport.
    /// </summary>
    public DateOnly ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the encrypted QR code payload for identity verification.
    /// </summary>
    public string? QrCodePayload { get; set; }

    /// <summary>
    /// Gets or sets the operational status of the passport (e.g. Active, Expired, Revoked).
    /// </summary>
    public PassportStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the passport authority branch identifier where the document was issued.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the name of the issuing branch.
    /// </summary>
    public string IssuingBranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the passport record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
