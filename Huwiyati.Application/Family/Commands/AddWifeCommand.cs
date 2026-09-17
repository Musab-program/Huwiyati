namespace Huwiyati.Application.Family.Commands;

// Command model carrying required data to add an additional Wife and Marriage Contract to an existing active Family Card via Husband's National Number
public class AddWifeCommand
{
    public string HusbandNationalNumber { get; set; } = string.Empty;
    public string WifeNationalNumber { get; set; } = string.Empty;
    public string MarriageContractNumber { get; set; } = string.Empty;
    public DateOnly MarriageDate { get; set; }
    public string? ContractPhotoUrl { get; set; }
    public Guid IssuingBranchId { get; set; }
}
