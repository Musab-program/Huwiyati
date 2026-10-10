namespace Huwiyati.Application.Documents.NationalIdCard.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;

public class GetActiveNationalIdCardByPersonIdHandler
{
    private readonly IApplicationDbContext _context;

    public GetActiveNationalIdCardByPersonIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<NationalIdCardDto>> GetByPersonIdAsync(
        Guid personId,
        CancellationToken cancellationToken = default)
    {
        if (personId == Guid.Empty)
        {
            return ApiResponse<NationalIdCardDto>.Failure("Invalid citizen identity.", statusCode: 400);
        }

        var cardDto = await _context.NationalIdCards
            .AsNoTracking()
            .Where(c => c.PersonId == personId && c.Status == NationalIdCardStatus.Active)
            .Select(c => new NationalIdCardDto
            {
                Id = c.Id,
                PersonId = c.PersonId,
                NationalNumber = c.Person.NationalNumber,
                FullName = $"{c.Person.FirstName} {c.Person.FatherName} {c.Person.GrandfatherName} {c.Person.FamilyName}".Trim(),
                PhotoUrl = c.Person.PhotoUrl,
                IssuingBranchId = c.IssuingBranchId,
                BranchName = c.OrganizationBranch.BranchName,
                IssueDate = c.IssueDate,
                ExpiryDate = c.ExpiryDate,
                QrCodePayload = c.QrCodePayload,
                Status = c.Status.ToString(),
                CreatedAt = c.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (cardDto == null)
        {
            return ApiResponse<NationalIdCardDto>.Failure(
                "Active National ID Card for this citizen was not found.",
                statusCode: 404);
        }

        return ApiResponse<NationalIdCardDto>.Success(cardDto);
    }
}
