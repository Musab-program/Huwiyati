namespace Huwiyati.Application.Organizations.Admins.Commands;

/// <summary>
/// Command to update the branch assignment of an administrator.
/// </summary>
public class UpdateAdminCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the destination organization branch.
    /// </summary>
    public Guid BranchId { get; set; }
}
