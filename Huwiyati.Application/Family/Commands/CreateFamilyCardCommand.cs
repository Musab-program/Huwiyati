namespace Huwiyati.Application.Family.Commands;

// Command model carrying required data to establish a new Family Record and Marriage Contract
public class CreateFamilyCardCommand
{
    // Husband & Wife National Numbers
    public string HusbandNationalNumber { get; set; } = string.Empty;
    public string WifeNationalNumber { get; set; } = string.Empty;

    // Marriage Contract Details
    public string MarriageContractNumber { get; set; } = string.Empty;
    public DateOnly MarriageDate { get; set; }
    public string? ContractPhotoUrl { get; set; }

    // Issuing Parameters
    public Guid IssuingBranchId { get; set; }
    
}