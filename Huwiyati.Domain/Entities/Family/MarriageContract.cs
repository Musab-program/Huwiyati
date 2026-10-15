namespace Huwiyati.Domain.Entities.Family;

using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Enums;

/// <summary>
/// Represents an official marriage contract document registered between a husband and a wife.
/// </summary>
public class MarriageContract : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique marriage contract certificate number.
    /// </summary>
    public string ContractNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the husband.
    /// </summary>
    public Guid HusbandPersonId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the wife.
    /// </summary>
    public Guid WifePersonId { get; set; }

    /// <summary>
    /// Gets or sets the official registration date of the marriage.
    /// </summary>
    public DateOnly MarriageDate { get; set; }

    /// <summary>
    /// Gets or sets the URL or storage path to an archival photograph of the marriage contract document.
    /// </summary>
    public string? DocumentPhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the current marital contract status (e.g., Active, Divorced, Annulled).
    /// </summary>
    public MarriageStatus Status { get; set; } = MarriageStatus.Active;

    /// <summary>
    /// Gets or sets the identifier of the authorized official who verified and approved the contract.
    /// </summary>
    public Guid ApprovedByUserId { get; set; }

    /// <summary>
    /// Gets or sets the husband person navigation entity.
    /// </summary>
    public Person HusbandPerson { get; set; } = null!;

    /// <summary>
    /// Gets or sets the wife person navigation entity.
    /// </summary>
    public Person WifePerson { get; set; } = null!;
}
