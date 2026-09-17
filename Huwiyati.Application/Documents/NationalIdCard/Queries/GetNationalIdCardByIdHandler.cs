namespace Huwiyati.Application.Documents.NationalIdCard.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;

// Handler returning single Active National ID Card details by Card ID
public class GetNationalIdCardByIdHandler
{
    private readonly IApplicationDbContext _context;

    public GetNationalIdCardByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<NationalIdCardDto>> GetNationalIdCardByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var cardDto = await _context.NationalIdCards
            .AsNoTracking()
            .Where(c => c.Id == id && c.Status == NationalIdCardStatus.Active)
            .Select(c => new NationalIdCardDto
            {
                Id = c.Id,
                PersonId = c.PersonId,
                NationalNumber = c.Person.NationalNumber,
                FullName = $"{c.Person.FirstName} {c.Person.FatherName} {c.Person.GrandfatherName} {c.Person.FamilyName}".Trim(),
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
                "Active National ID Card with the specified ID was not found.",
                statusCode: 404);
        }

        return ApiResponse<NationalIdCardDto>.Success(cardDto);
    }
}
