namespace Huwiyati.Application.Family.Commands;

// Command model for renewing an existing Family Card
public class RenewFamilyCardCommand
{
    public string FamilyNumber { get; set; } = string.Empty;
    public Guid IssuingBranchId { get; set; }
}