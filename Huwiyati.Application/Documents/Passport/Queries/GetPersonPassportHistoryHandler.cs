namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;

// Handler returning full passport history (Active, Expired, Canceled) for a citizen by National Number
public class GetPersonPassportHistoryHandler
{
    private readonly IApplicationDbContext _context;

    public GetPersonPassportHistoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<PassportDto>>> GetPassportHistoryByNationalNumberAsync(
        string nationalNumber,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify citizen exists in Civil Registry
        var personExists = await _context.Persons
            .AsNoTracking()
            .AnyAsync(p => p.NationalNumber == nationalNumber, cancellationToken);

        if (!personExists)
        {
            return ApiResponse<List<PassportDto>>.Failure(
                "No citizen record found with the provided National Number.",
                statusCode: 404);
        }

        // 2. Fetch full passport history (Active, Expired, Canceled)
        var passportHistory = await _context.Passports
            .AsNoTracking()
            .Where(p => p.Person.NationalNumber == nationalNumber)
            .OrderByDescending(p => p.IssueDate)
            .Select(p => new PassportDto
            {
                Id = p.Id,
                PersonId = p.PersonId,
                PersonFullName = $"{p.Person.FirstName} {p.Person.FatherName} {p.Person.GrandfatherName} {p.Person.FamilyName}".Trim(),
                NationalNumber = p.Person.NationalNumber,
                PhotoUrl = p.Person.PhotoUrl,
                PassportNumber = p.PassportNumber,
                PassportType = p.PassportType,
                IssueDate = p.IssueDate,
                ExpiryDate = p.ExpiryDate,
                QrCodePayload = p.QrCodePayload,
                Status = p.Status,
                IssuingBranchId = p.IssuingBranchId,
                IssuingBranchName = p.IssuingBranch.BranchName,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<PassportDto>>.Success(passportHistory);
    }
}
