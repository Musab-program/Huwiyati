namespace Huwiyati.Application.Family.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Family.DTOs;

/// <summary>
/// Handles querying detailed family information by its unique identifier.
/// </summary>
public class GetFamilyByIdHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFamilyByIdHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetFamilyByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a single family register record along with its head and member details projected into <see cref="FamilyDto"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the family record.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the matching <see cref="FamilyDto"/> or a 404 response.</returns>
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
