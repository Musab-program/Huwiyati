namespace Huwiyati.Application.Family.Commands;

using Huwiyati.Domain.Enums;

/// <summary>
/// Command payload for updating the status of an individual family member (e.g. Left, Active, Divorced, Deceased).
/// </summary>
public class UpdateFamilyMemberStatusCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the family membership record.
    /// </summary>
    public Guid FamilyMemberId { get; set; }

    /// <summary>
    /// Gets or sets the new status of the member within the family.
    /// </summary>
    public FamilyMemberStatus NewStatus { get; set; }

    /// <summary>
    /// Gets or sets the optional timestamp when the member left or separated from the family.
    /// </summary>
    public DateTime? LeftAt { get; set; }
}