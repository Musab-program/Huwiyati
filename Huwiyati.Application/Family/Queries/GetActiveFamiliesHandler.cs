namespace Huwiyati.Application.Family.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Family.DTOs;
using Huwiyati.Domain.Enums;

/// <summary>
/// Handles querying all active family units across the civil registry.
/// </summary>
public class GetActiveFamiliesHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetActiveFamiliesHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetActiveFamiliesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all active family summaries including head of family details and member counts.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of <see cref="FamilySummaryDto"/>.</returns>
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
