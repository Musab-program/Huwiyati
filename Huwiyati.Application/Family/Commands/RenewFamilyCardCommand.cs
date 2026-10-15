namespace Huwiyati.Application.Family.Commands;

/// <summary>
/// Command payload for renewing an existing family card record.
/// </summary>
public class RenewFamilyCardCommand
{
    /// <summary>
    /// Gets or sets the official family register number to be renewed.
    /// </summary>
    public string FamilyNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the civil registry branch identifier processing the renewal.
    /// </summary>
    public Guid IssuingBranchId { get; set; }
}