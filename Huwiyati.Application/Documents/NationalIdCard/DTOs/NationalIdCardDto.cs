namespace Huwiyati.Application.Documents.NationalIdCard.DTOs;

/// <summary>
/// Represents data transfer object containing National ID card details returned to clients.
/// </summary>
public class NationalIdCardDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the national ID card record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique person identifier of the cardholder.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the national number of the cardholder.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full legal name of the cardholder.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL or path to the cardholder's biometric photo.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the civil registry branch identifier where the card was issued.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the name of the issuing branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the official issuance date of the card.
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the card.
    /// </summary>
    public DateOnly ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the encrypted QR code verification payload.
    /// </summary>
    public string? QrCodePayload { get; set; }

    /// <summary>
    /// Gets or sets the textual representation of the card status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the card record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
