namespace Huwiyati.Application.Documents.NationalIdCard.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;

// Handler returning full card history (Active, Expired, Lost, Suspended) for a citizen by National Number
public class GetPersonNationalIdCardHistoryHandler
{
    private readonly IApplicationDbContext _context;

    public GetPersonNationalIdCardHistoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<NationalIdCardDto>>> GetCardHistoryByNationalNumberAsync(
        string nationalNumber,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify citizen exists in Civil Registry
        var personExists = await _context.Persons
            .AsNoTracking()
            .AnyAsync(p => p.NationalNumber == nationalNumber, cancellationToken);

        if (!personExists)
        {
            return ApiResponse<List<NationalIdCardDto>>.Failure(
                "No citizen record found with the provided National Number in Civil Registry.",
                statusCode: 404);
        }

        // 2. Fetch full card history (Active, Expired, Lost, Suspended)
        var cardHistory = await _context.NationalIdCards
            .AsNoTracking()
            .Where(c => c.Person.NationalNumber == nationalNumber)
            .OrderByDescending(c => c.IssueDate)
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

        return ApiResponse<List<NationalIdCardDto>>.Success(cardHistory);
    }
}
