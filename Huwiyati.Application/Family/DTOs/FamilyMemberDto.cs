namespace Huwiyati.Application.Family.DTOs;

/// <summary>
/// Represents data transfer object for an individual family member within a family register.
/// </summary>
public class FamilyMemberDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the family membership record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique person identifier of the family member.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the national number of the member.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full legal name of the member.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of birth of the member.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the relationship type to the head of family (e.g. Wife, Son, Daughter).
    /// </summary>
    public string RelationshipType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the membership status (e.g. Active, Separated, Deceased).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional marriage contract identifier for spouses.
    /// </summary>
    public Guid? MarriageContractId { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the member was added to the family.
    /// </summary>
    public DateTime JoinedAt { get; set; }

    /// <summary>
    /// Gets or sets the optional timestamp when the member departed or left the family.
    /// </summary>
    public DateTime? LeftAt { get; set; }
}