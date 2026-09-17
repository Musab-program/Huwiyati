namespace Huwiyati.Application.Documents.NationalIdCard.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;

// Handler returning list of active National ID Cards directly using AsNoTracking and Select projection
public class GetNationalIdCardsHandler
{
    private readonly IApplicationDbContext _context;

    public GetNationalIdCardsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<NationalIdCardDto>>> GetActiveNationalIdCardsAsync(
        CancellationToken cancellationToken = default)
    {
        var activeCards = await _context.NationalIdCards
            .AsNoTracking()
            .Where(c => c.Status == NationalIdCardStatus.Active)
            .OrderByDescending(c => c.CreatedAt)
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
            .ToListAsync(cancellationToken);

        return ApiResponse<List<NationalIdCardDto>>.Success(activeCards);
    }
}
