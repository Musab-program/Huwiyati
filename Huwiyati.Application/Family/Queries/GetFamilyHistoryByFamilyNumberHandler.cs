namespace Huwiyati.Application.Family.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Family.DTOs;

public class GetFamilyHistoryByFamilyNumberHandler
{
    private readonly IApplicationDbContext _context;

    public GetFamilyHistoryByFamilyNumberHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<FamilyDto>>> GetHistoryAsync(string familyNumber, CancellationToken cancellationToken = default)
    {
        var history = await _context.Families
            .AsNoTracking()
            .Where(f => f.FamilyNumber == familyNumber)
            .OrderByDescending(f => f.IssueDate)
            .Select(f => new FamilyDto
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
                QrCodePayload = f.QrCodePayload,
                Status = f.Status.ToString(),
                CreatedAt = f.CreatedAt,
                Members = f.FamilyMembers.Select(m => new FamilyMemberDto
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
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        if (history == null || !history.Any())
        {
            return ApiResponse<List<FamilyDto>>.Failure(
                "No family records history found for the provided Family Number.", statusCode: 404);
        }

        return ApiResponse<List<FamilyDto>>.Success(
            history, message: "Family card history retrieved successfully.", statusCode: 200);
    }
}
