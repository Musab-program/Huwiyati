namespace Huwiyati.Application.Family.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Family.DTOs;
using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Enums;

public class CreateFamilyCardHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IDocumentNumberGenerator _documentNumberGenerator;

    public CreateFamilyCardHandler(
        IApplicationDbContext context,
        IDocumentNumberGenerator documentNumberGenerator)
    {
        _context = context;
        _documentNumberGenerator = documentNumberGenerator;
    }

    public async Task<ApiResponse<FamilyDto>> CreateAsync(
        CreateFamilyCardCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify issuing branch exists, is active, and belongs to Civil Registry organization ("الأحوال المدنية") using Select
        var branchData = await _context.OrganizationBranches
            .AsNoTracking()
            .Where(b => b.Id == command.IssuingBranchId)
            .Select(b => new
            {
                b.Id,
                b.BranchName,
                b.IsActive,
                OrganizationIsActive = b.Organization.IsActive,
                OrganizationName = b.Organization.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (branchData == null)
        {
            return ApiResponse<FamilyDto>.Failure(
                "Specified issuing branch does not exist.", statusCode: 404);
        }

        if (!branchData.IsActive || !branchData.OrganizationIsActive)
        {
            return ApiResponse<FamilyDto>.Failure(
                "Specified issuing branch or its parent organization is inactive.", statusCode: 400);
        }

        if (!branchData.OrganizationName.Contains("الأحوال المدنية"))
        {
            return ApiResponse<FamilyDto>.Failure(
                "Family Cards can only be issued by Civil Registry branches (الأحوال المدنية).", statusCode: 400);
        }

        // 2. Fetch Husband record from Civil Registry
        var husband = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.HusbandNationalNumber, cancellationToken);

        if (husband == null)
        {
            return ApiResponse<FamilyDto>.Failure(
                "Husband citizen record was not found in Civil Registry.", statusCode: 404);
        }

        // 3. Fetch Wife record from Civil Registry
        var wife = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.WifeNationalNumber, cancellationToken);

        if (wife == null)
        {
            return ApiResponse<FamilyDto>.Failure(
                "Wife citizen record was not found in Civil Registry.", statusCode: 404);
        }

        // 4. Verify Husband does not already possess an active Family Record as Head
        var activeHeadFamilyExists = await _context.Families
            .AsNoTracking()
            .AnyAsync(f => f.HeadOfFamilyPersonId == husband.Id && f.Status == FamilyStatus.Active, cancellationToken);

        if (activeHeadFamilyExists)
        {
            return ApiResponse<FamilyDto>.Failure(
                "Husband already possesses an active Family Record. Use Add Wife service to register additional wives.", statusCode: 400);
        }

        // 5. Verify Wife is not currently registered as an active wife in another active family record
        var isWifeCurrentlyMarried = await _context.FamilyMembers
            .AsNoTracking()
            .AnyAsync(m => m.PersonId == wife.Id
                           && m.RelationshipType == RelationshipType.Wife
                           && m.Status == FamilyMemberStatus.Active, cancellationToken);

        if (isWifeCurrentlyMarried)
        {
            return ApiResponse<FamilyDto>.Failure(
                "The specified wife is currently registered as an active wife in another family record. Citizen must be divorced before creating a new marriage contract.", statusCode: 400);
        }

        // 6. Generate unique 11-digit Family Number starting with '02'
        var familyNumber = await _documentNumberGenerator.GenerateFamilyNumberAsync(command.IssuingBranchId, cancellationToken);

        // 7. Create MarriageContract document
        var marriageContract = new MarriageContract
        {
            ContractNumber = command.MarriageContractNumber,
            HusbandPersonId = husband.Id,
            WifePersonId = wife.Id,
            MarriageDate = command.MarriageDate,
            DocumentPhotoUrl = command.ContractPhotoUrl,
            Status = MarriageStatus.Active,
            ApprovedByUserId = Guid.Empty,
            CreatedAt = DateTime.UtcNow,
        };

        await _context.MarriageContracts.AddAsync(marriageContract, cancellationToken);

        // 8. Create Family entity
        var issueDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var expiryDate = issueDate.AddYears(10);

        var family = new Family
        {
            FamilyNumber = familyNumber,
            HeadOfFamilyPersonId = husband.Id,
            IssuingBranchId = command.IssuingBranchId,
            IssueDate = issueDate,
            ExpiryDate = expiryDate,
            QrCodePayload = $"FAM-{familyNumber}",
            Status = FamilyStatus.Active,
            CreatedAt = DateTime.UtcNow,
        };

        await _context.Families.AddAsync(family, cancellationToken);

        // 9. Add Husband (Head) and Wife as initial Family Members
        var headMember = new FamilyMember
        {
            FamilyId = family.Id,
            PersonId = husband.Id,
            MarriageContractId = null,
            RelationshipType = RelationshipType.Head,
            Status = FamilyMemberStatus.Active,
            CreatedAt = DateTime.UtcNow,
            JoinedAt = DateTime.UtcNow
        };

        var wifeMember = new FamilyMember
        {
            FamilyId = family.Id,
            PersonId = wife.Id,
            MarriageContractId = marriageContract.Id,
            RelationshipType = RelationshipType.Wife,
            Status = FamilyMemberStatus.Active,
            CreatedAt = DateTime.UtcNow,
            JoinedAt = DateTime.UtcNow
        };

        await _context.FamilyMembers.AddRangeAsync(new[] { headMember, wifeMember }, cancellationToken);

        // 10. Update MaritalStatus for both Husband and Wife to Married
        husband.MaritalStatus = MaritalStatus.Married;
        wife.MaritalStatus = MaritalStatus.Married;

        

        await _context.SaveChangesAsync(cancellationToken);

        // 11. Construct response DTO
        var responseDto = new FamilyDto
        {
            Id = family.Id,
            FamilyNumber = family.FamilyNumber,
            HeadOfFamilyPersonId = husband.Id,
            HeadOfFamilyNationalNumber = husband.NationalNumber,
            HeadOfFamilyFullName = $"{husband.FirstName} {husband.FatherName} {husband.GrandfatherName} {husband.FamilyName}".Trim(),
            IssuingBranchId = branchData.Id,
            BranchName = branchData.BranchName,
            IssueDate = family.IssueDate,
            ExpiryDate = family.ExpiryDate,
            QrCodePayload = family.QrCodePayload,
            Status = family.Status.ToString(),
            CreatedAt = family.CreatedAt,
            Members = new List<FamilyMemberDto>
            {
                new FamilyMemberDto
                {
                    Id = headMember.Id,
                    PersonId = husband.Id,
                    NationalNumber = husband.NationalNumber,
                    FullName = $"{husband.FirstName} {husband.FatherName} {husband.GrandfatherName} {husband.FamilyName}".Trim(),
                    DateOfBirth = husband.DateOfBirth,
                    RelationshipType = headMember.RelationshipType.ToString(),
                    Status = headMember.Status.ToString(),
                    MarriageContractId = null,
                    JoinedAt = headMember.JoinedAt,
                    LeftAt = headMember.LeftAt
                },
                new FamilyMemberDto
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
                    LeftAt = wifeMember.LeftAt
                }
            }
        };

        return ApiResponse<FamilyDto>.Success(
            responseDto, message: "Family Record and Card created successfully.", statusCode: 201);
    }
}