namespace Huwiyati.Application.Family.Commands;

/// <summary>
/// Command payload for establishing a new family record and issuing an initial family card.
/// </summary>
public class CreateFamilyCardCommand
{
    /// <summary>
    /// Gets or sets the national number of the husband (head of the family).
    /// </summary>
    public string HusbandNationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the national number of the wife.
    /// </summary>
    public string WifeNationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the official marriage contract document number.
    /// </summary>
    public string MarriageContractNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the formal date of marriage.
    /// </summary>
    public DateOnly MarriageDate { get; set; }

    /// <summary>
    /// Gets or sets an optional scanned photo or document URL of the marriage contract.
    /// </summary>
    public string? ContractPhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the civil registry branch identifier issuing the card.
    /// </summary>
    public Guid IssuingBranchId { get; set; }
}