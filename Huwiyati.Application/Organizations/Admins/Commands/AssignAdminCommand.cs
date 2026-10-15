namespace Huwiyati.Application.Organizations.Admins.Commands;

/// <summary>
/// Command to assign administrative rights and employee registration to a citizen for a specific branch.
/// </summary>
public class AssignAdminCommand
{
    /// <summary>
    /// Gets or sets the national identification number of the citizen to be assigned as admin.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the target organization branch.
    /// </summary>
    public Guid BranchId { get; set; }
}
