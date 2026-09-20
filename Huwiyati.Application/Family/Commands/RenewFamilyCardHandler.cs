namespace Huwiyati.Application.Family.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Family.DTOs;
using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Common.Extensions;

public class RenewFamilyCardHandler
{
    private readonly IApplicationDbContext _context;

    public RenewFamilyCardHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<FamilyDto>> RenewAsync(
        RenewFamilyCardCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify issuing branch exists, is active, and belongs to Civil Registry ("الأحوال المدنية") via Select
        var branchData = await _context.ValidateCivilRegistryBranchAsync(command.IssuingBranchId, cancellationToken);
        if(!branchData.IsValid)
        {
            return ApiResponse<FamilyDto>.Failure(
                branchData.ErrorMessage,
                statusCode: branchData.StatusCode
                );
        }

        // 2. Find target Family record by FamilyNumber
        var existingFamily = await _context.Families
            .FirstOrDefaultAsync(f => f.FamilyNumber == command.FamilyNumber && f.Status == FamilyStatus.Active, cancellationToken)
            ?? await _context.Families.FirstOrDefaultAsync(f => f.FamilyNumber == command.FamilyNumber, cancellationToken);

        if (existingFamily == null)
        {
            return ApiResponse<FamilyDto>.Failure(
                "No Family record found with the provided Family Number.", statusCode: 404);
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // 3. If the card is currently Active, check if it is within 90 days of expiry
        if (existingFamily.Status == FamilyStatus.Active)
        {
            var daysRemaining = existingFamily.ExpiryDate.DayNumber - today.DayNumber;

            if (daysRemaining > 90)
            {
                return ApiResponse<FamilyDto>.Failure(
                    $"Family card cannot be renewed yet. Renewal is only allowed within 3 months (90 days) prior to expiry date. Remaining valid days: {daysRemaining}.",
                    statusCode: 400);
            }

            // Mark previous active Family record as Expired
            existingFamily.Status = FamilyStatus.Expired;
        }

        // 4. Create NEW Family card entity with renewed 10-year validity dates
        var newFamily = new Family
        {
            FamilyNumber = existingFamily.FamilyNumber,
            HeadOfFamilyPersonId = existingFamily.HeadOfFamilyPersonId,
            IssuingBranchId = command.IssuingBranchId,
            IssueDate = today,
            ExpiryDate = today.AddYears(10),
            QrCodePayload = $"FAM-{existingFamily.FamilyNumber}",
            Status = FamilyStatus.Active,
            CreatedAt = DateTime.UtcNow,
        };

        await _context.Families.AddAsync(newFamily, cancellationToken);

        // 5. Re-link active members to the new Family Card by cloning FamilyMember records for historical integrity
        var activeMembers = await _context.FamilyMembers
            .Where(m => m.FamilyId == existingFamily.Id && m.Status == FamilyMemberStatus.Active)
            .ToListAsync(cancellationToken);

        var newMembers = activeMembers.Select(m => new FamilyMember
        {
            Id = Guid.NewGuid(),
            FamilyId = newFamily.Id,
            PersonId = m.PersonId,
            MarriageContractId = m.MarriageContractId,
            RelationshipType = m.RelationshipType,
            Status = FamilyMemberStatus.Active,
            JoinedAt = today.ToDateTime(TimeOnly.MinValue)
        }).ToList();

        await _context.FamilyMembers.AddRangeAsync(newMembers, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 6. Fetch updated members for response projection via Select (Zero Include)
        var responseMembersDtoList = await _context.FamilyMembers
            .AsNoTracking()
            .Where(m => m.FamilyId == newFamily.Id && m.Status == FamilyMemberStatus.Active)
            .Select(m => new FamilyMemberDto
            {
                Id = m.Id,
                PersonId = m.PersonId,
                NationalNumber = m.Person.NationalNumber,
                FullName = $"{m.Person.FirstName} {m.Person.FatherName} {m.Person.GrandfatherName} {m.Person.FamilyName}".Trim(),
                DateOfBirth = m.Person.DateOfBirth,
                RelationshipType = m.RelationshipType.ToString(),
                Status = m.Status.ToString(),
                MarriageContractId = m.MarriageContractId,
                JoinedAt = m.JoinedAt,
                LeftAt = m.LeftAt
            })
            .ToListAsync(cancellationToken);

        // 7. Get Head of Family details for response projection
        var headMemberDto = responseMembersDtoList.FirstOrDefault(m => m.RelationshipType == RelationshipType.Head.ToString());

        var responseDto = new FamilyDto
        {
            Id = newFamily.Id,
            FamilyNumber = newFamily.FamilyNumber,
            HeadOfFamilyPersonId = newFamily.HeadOfFamilyPersonId,
            HeadOfFamilyNationalNumber = headMemberDto?.NationalNumber ?? string.Empty,
            HeadOfFamilyFullName = headMemberDto?.FullName ?? string.Empty,
            IssuingBranchId = branchData.BranchId,
            BranchName = branchData.BranchName,
            IssueDate = newFamily.IssueDate,
            ExpiryDate = newFamily.ExpiryDate,
            QrCodePayload = newFamily.QrCodePayload,
            Status = newFamily.Status.ToString(),
            CreatedAt = newFamily.CreatedAt,
            Members = responseMembersDtoList
        };

        return ApiResponse<FamilyDto>.Success(
            responseDto, message: "Family Card renewed successfully with a new 10-year validity period.", statusCode: 200);
    }
}