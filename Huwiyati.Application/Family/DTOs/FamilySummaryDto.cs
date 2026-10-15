namespace Huwiyati.Application.Family.DTOs;

/// <summary>
/// Lightweight summary data transfer object for paginated lists and tabular displays of family units.
/// </summary>
public class FamilySummaryDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the family.
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
    /// Gets or sets the issuing civil registry branch identifier.
    /// </summary>
    public Guid IssuingBranchId { get; set; }

    /// <summary>
    /// Gets or sets the name of the issuing branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the family card issue date.
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the family card expiry date.
    /// </summary>
    public DateOnly ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the status string of the family.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total count of currently active members in the family.
    /// </summary>
    public int ActiveMembersCount { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp of the family record.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
