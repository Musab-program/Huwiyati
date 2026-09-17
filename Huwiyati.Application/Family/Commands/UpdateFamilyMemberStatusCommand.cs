namespace Huwiyati.Application.Family.Commands;

using Huwiyati.Domain.Enums;

// Command to update status of an individual family member (Left, Active, Divorced, Deceased) with optional custom departure date
public class UpdateFamilyMemberStatusCommand
{
    public Guid FamilyMemberId { get; set; }
    public FamilyMemberStatus NewStatus { get; set; }
    public DateTime? LeftAt { get; set; }
}