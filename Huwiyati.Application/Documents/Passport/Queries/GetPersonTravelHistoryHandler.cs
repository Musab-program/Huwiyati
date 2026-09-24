namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;

// Handler returning full lifetime travel history for a citizen by National Number across all passports
public class GetPersonTravelHistoryHandler
{
    private readonly IApplicationDbContext _context;

    public GetPersonTravelHistoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<TravelRecordDto>>> GetTravelHistoryByNationalNumberAsync(
        string nationalNumber,
        CancellationToken cancellationToken = default)
    {
        var personExists = await _context.Persons
            .AsNoTracking()
            .AnyAsync(p => p.NationalNumber == nationalNumber, cancellationToken);

        if (!personExists)
        {
            return ApiResponse<List<TravelRecordDto>>.Failure(
                "No person record found with the provided National Number.", statusCode: 404);
        }

        var records = await _context.TravelRecords
            .AsNoTracking()
            .Where(t => t.Passport.Person.NationalNumber == nationalNumber)
            .OrderByDescending(t => t.EntryDate)
            .Select(t => new TravelRecordDto
            {
                Id = t.Id,
                PassportId = t.PassportId,
                PassportNumber = t.Passport.PassportNumber,
                PersonId = t.Passport.PersonId,
                PersonFullName = $"{t.Passport.Person.FirstName} {t.Passport.Person.FatherName} {t.Passport.Person.GrandfatherName} {t.Passport.Person.FamilyName}".Trim(),
                NationalNumber = t.Passport.Person.NationalNumber,
                IssuingBranchId = t.IssuingBranchId,
                IssuingBranchName = t.IssuingBranch.BranchName,
                Country = t.Country,
                EntryDate = t.EntryDate,
                ExitDate = t.ExitDate,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<TravelRecordDto>>.Success(records);
    }
}
