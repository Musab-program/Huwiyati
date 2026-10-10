namespace Huwiyati.Application.Family.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Family.DTOs;
using Huwiyati.Domain.Enums;

public class GetCitizenFamilyCardsHandler
{
    private readonly IApplicationDbContext _context;

    public GetCitizenFamilyCardsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<FamilyDto>>> GetCitizenFamiliesAsync(Guid citizenPersonId, CancellationToken cancellationToken = default)
    {
        if (citizenPersonId == Guid.Empty)
        {
            return ApiResponse<List<FamilyDto>>.Failure("Invalid or unassociated citizen identity.", statusCode: 400);
        }

        var familyDtos = await _context.Families
            .AsNoTracking()
            .Where(f => (f.HeadOfFamilyPersonId == citizenPersonId || f.FamilyMembers.Any(m => m.PersonId == citizenPersonId)) && f.Status == FamilyStatus.Active)
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

        return ApiResponse<List<FamilyDto>>.Success(
            familyDtos, message: "Citizen family card records retrieved successfully.", statusCode: 200);
    }
}
