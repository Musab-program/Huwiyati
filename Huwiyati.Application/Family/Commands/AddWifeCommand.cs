namespace Huwiyati.Application.Family.Commands;

/// <summary>
/// Command payload for adding a new wife and registering an official marriage contract in the family.
/// </summary>
public class AddWifeCommand
{
    /// <summary>
    /// Gets or sets the national number of the husband (head of family).
    /// </summary>
    public string HusbandNationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the national number of the wife to be added.
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
    /// Gets or sets the civil registry branch identifier registering the marriage.
    /// </summary>
    public Guid IssuingBranchId { get; set; }
}
