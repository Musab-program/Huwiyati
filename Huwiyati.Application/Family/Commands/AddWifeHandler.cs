namespace Huwiyati.Application.Family.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Family.DTOs;
using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Common.Extensions;

public class AddWifeHandler
{
    private readonly IApplicationDbContext _context;

    public AddWifeHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<FamilyMemberDto>> AddWifeAsync(
        AddWifeCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify issuing branch exists, is active, and belongs to Civil Registry ("الأحوال المدنية") using Select
        var branchData = await _context.ValidateCivilRegistryBranchAsync(command.IssuingBranchId, cancellationToken);
        if (!branchData.IsValid)
        {
            return ApiResponse<FamilyMemberDto>.Failure(
                     branchData.ErrorMessage,
                    statusCode: branchData.StatusCode);
        }
        

        // 2. Fetch Husband citizen record
        var husband = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.HusbandNationalNumber, cancellationToken);

        if (husband == null)
        {
            return ApiResponse<FamilyMemberDto>.Failure(
                "Husband citizen record was not found in Civil Registry.", statusCode: 404);
        }

        // 3. Fetch Husband's active Family record
        var family = await _context.Families
            .FirstOrDefaultAsync(f => f.HeadOfFamilyPersonId == husband.Id && f.Status == FamilyStatus.Active, cancellationToken);

        if (family == null)
        {
            return ApiResponse<FamilyMemberDto>.Failure(
                "Active Family record for the husband was not found.", statusCode: 404);
        }

        // 4. Fetch Wife citizen record from Civil Registry
        var wife = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.WifeNationalNumber, cancellationToken);

        if (wife == null)
        {
            return ApiResponse<FamilyMemberDto>.Failure(
                "Wife citizen record was not found in Civil Registry.", statusCode: 404);
        }

        // 5. Verify Wife is not currently registered as an active wife in another family card
        var isWifeAlreadyMarried = await _context.FamilyMembers
            .AsNoTracking()
            .AnyAsync(m => m.PersonId == wife.Id 
                           && m.RelationshipType == RelationshipType.Wife 
                           && m.Status == FamilyMemberStatus.Active, cancellationToken);

        if (isWifeAlreadyMarried)
        {
            return ApiResponse<FamilyMemberDto>.Failure(
                "The specified wife is currently registered as an active wife in another family record.", statusCode: 400);
        }

        // 5.1 Verify the ContractNumber has not been used
        var isExistContractNumber = await _context.MarriageContracts.
                AnyAsync(x => x.ContractNumber == command.MarriageContractNumber);
        if (isExistContractNumber)
        {
            return ApiResponse<FamilyMemberDto>.Failure(
                "The ContractNumber is currently used with another Marriage contranct record.", statusCode: 400);
        }

        // 6. Create new MarriageContract record
        var marriageContract = new MarriageContract
        {
            ContractNumber = command.MarriageContractNumber,
            HusbandPersonId = husband.Id,
            WifePersonId = wife.Id,
            MarriageDate = command.MarriageDate,
            DocumentPhotoUrl = command.ContractPhotoUrl,
            Status = MarriageStatus.Active,
            ApprovedByUserId = Guid.Empty
        };

        await _context.MarriageContracts.AddAsync(marriageContract, cancellationToken);

        // 7. Create new FamilyMember entity for the Wife
        var wifeMember = new FamilyMember
        {
            FamilyId = family.Id,
            PersonId = wife.Id,
            MarriageContractId = marriageContract.Id,
            RelationshipType = RelationshipType.Wife,
            Status = FamilyMemberStatus.Active,
            JoinedAt = DateTime.UtcNow
        };

        await _context.FamilyMembers.AddAsync(wifeMember, cancellationToken);

        // 8. Update Wife MaritalStatus in Civil Registry
        wife.MaritalStatus = MaritalStatus.Married;

        await _context.SaveChangesAsync(cancellationToken);

        // 9. Construct response DTO
        var responseDto = new FamilyMemberDto
        {
            Id = wifeMember.Id,
            PersonId = wife.Id,
            NationalNumber = wife.NationalNumber,
            FullName = $"{wife.FirstName} {wife.FatherName} {wife.GrandfatherName} {wife.FamilyName}".Trim(),
            DateOfBirth = wife.DateOfBirth,
            RelationshipType = wifeMember.RelationshipType.ToString(),
            Status = wifeMember.Status.ToString(),
            MarriageContractId = marriageContract.Id,
            JoinedAt = wifeMember.JoinedAt,
            LeftAt = null
        };

        return ApiResponse<FamilyMemberDto>.Success(
            responseDto, message: "Additional wife registered and added to Family Card successfully.", statusCode: 201);
    }
}
