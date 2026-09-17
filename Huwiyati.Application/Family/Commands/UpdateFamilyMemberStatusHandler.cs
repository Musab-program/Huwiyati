namespace Huwiyati.Application.Family.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Enums;

public class UpdateFamilyMemberStatusHandler
{
    private readonly IApplicationDbContext _context;

    public UpdateFamilyMemberStatusHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> UpdateStatusAsync(
        UpdateFamilyMemberStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Fetch FamilyMember entity by ID
        var member = await _context.FamilyMembers
            .FirstOrDefaultAsync(m => m.Id == command.FamilyMemberId, cancellationToken);

        if (member == null)
        {
            return ApiResponse<bool>.Failure(
                "Specified family member record was not found.", statusCode: 404);
        }

        // 2. Prevent head of family status from being modified directly
        if (member.RelationshipType == RelationshipType.Head)
        {
            return ApiResponse<bool>.Failure(
                "Head of family member status cannot be modified directly.", statusCode: 400);
        }

        // 3. Update status and LeftAt timestamp logic
        member.Status = command.NewStatus;
        member.LastModifiedAt = DateTime.UtcNow;
        if (command.NewStatus == FamilyMemberStatus.Active)
        {
            member.LeftAt = null;
        }
        else
        {
            member.LeftAt = command.LeftAt ?? DateTime.UtcNow;
        }

        // 4. Handle Divorce scenario: If member is Wife and status is Divorced
        if (command.NewStatus == FamilyMemberStatus.Divorced && member.RelationshipType == RelationshipType.Wife)
        {
            // Update linked MarriageContract status to Divorced
            if (member.MarriageContractId.HasValue)
            {
                var contract = await _context.MarriageContracts
                    .FirstOrDefaultAsync(c => c.Id == member.MarriageContractId.Value, cancellationToken);

                if (contract != null)
                {
                    contract.Status = MarriageStatus.Divorced;
                    contract.LastModifiedAt = DateTime.UtcNow;
                }
            }

            // Update Wife's MaritalStatus in Civil Registry
            var wifePerson = await _context.Persons
                .FirstOrDefaultAsync(p => p.Id == member.PersonId, cancellationToken);

            if (wifePerson != null)
            {
                wifePerson.MaritalStatus = MaritalStatus.Divorced;
                wifePerson.LastModifiedAt = DateTime.UtcNow;
            }
        }

        // 5. Handle Deceased scenario: Update Person status in Civil Registry
        if (command.NewStatus == FamilyMemberStatus.Deceased)
        {
            var person = await _context.Persons
                .FirstOrDefaultAsync(p => p.Id == member.PersonId, cancellationToken);

            if (person != null)
            {
                person.PersonStatus = PersonStatus.Deceased;
                person.LastModifiedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Success(
            true, message: "Family member status and related legal records updated successfully.", statusCode: 200);
    }
}
