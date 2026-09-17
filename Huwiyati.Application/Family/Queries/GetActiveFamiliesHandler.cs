namespace Huwiyati.Application.Family.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Family.DTOs;
using Huwiyati.Domain.Enums;

public class GetActiveFamiliesHandler
{
    private readonly IApplicationDbContext _context;

    public GetActiveFamiliesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<FamilySummaryDto>>> GetActiveFamiliesAsync(CancellationToken cancellationToken = default)
    {
        var activeFamilies = await _context.Families
            .AsNoTracking()
            .Where(f => f.Status == FamilyStatus.Active)
            .Select(f => new FamilySummaryDto
            {
                Id = f.Id,
                FamilyNumber = f.FamilyNumber,
                HeadOfFamilyPersonId = f.HeadOfFamilyPersonId,
                HeadOfFamilyNationalNumber = f.HeadOfFamily.NationalNumber,
                HeadOfFamilyFullName = $"{f.HeadOfFamily.FirstName} {f.HeadOfFamily.FatherName} {f.HeadOfFamily.GrandfatherName} {f.HeadOfFamily.FamilyName}".Trim(),
                IssuingBranchId = f.IssuingBranchId,
                BranchName = f.IssuingBranch.BranchName,
                IssueDate = f.IssueDate,
                ExpiryDate = f.ExpiryDate,
                Status = f.Status.ToString(),
                ActiveMembersCount = f.FamilyMembers.Count(m => m.Status == FamilyMemberStatus.Active),
                CreatedAt = f.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<FamilySummaryDto>>.Success(
            activeFamilies, message: "Active family records retrieved successfully.", statusCode: 200);
    }
}
