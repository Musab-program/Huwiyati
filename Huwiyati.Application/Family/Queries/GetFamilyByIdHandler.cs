namespace Huwiyati.Application.Family.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Family.DTOs;

public class GetFamilyByIdHandler
{
    private readonly IApplicationDbContext _context;

    public GetFamilyByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<FamilyDto>> GetFamilyByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var familyDto = await _context.Families
            .AsNoTracking()
            .Where(f => f.Id == id)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (familyDto == null)
        {
            return ApiResponse<FamilyDto>.Failure(
                "Specified Family record was not found.", statusCode: 404);
        }

        return ApiResponse<FamilyDto>.Success(
            familyDto, message: "Family record details retrieved successfully.", statusCode: 200);
    }
}
