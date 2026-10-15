namespace Huwiyati.Application.Family.DTOs;
/// <summary>
/// Represents data transfer object for a family unit and its registered family card details.
/// </summary>
public class FamilyDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the family record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the official family register number.
    /// </summary>
    public string FamilyNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the person identifier of the head of family.
    /// </summary>
    public Guid HeadOfFamilyPersonId { get; set; }

    /// <summary>
    /// Gets or sets the national number of the head of family.
    /// </summary>
    public string HeadOfFamilyNationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full legal name of the head of family.
    /// </summary>
    public string HeadOfFamilyFullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the civil registry branch identifier where the family card was issued.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the name of the issuing branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the issuance date of the family card.
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the family card.
    /// </summary>
    public DateOnly ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the encrypted QR code payload for verification.
    /// </summary>
    public string? QrCodePayload { get; set; }

    /// <summary>
    /// Gets or sets the status of the family unit (e.g. Active, Closed).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the family record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the collection of family members affiliated with this family.
    /// </summary>
    public List<FamilyMemberDto> Members { get; set; } = new List<FamilyMemberDto>();
}

