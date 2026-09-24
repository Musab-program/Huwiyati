namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;

// Handler returning list of travel records for a specific Passport Number
public class GetPassportTravelRecordsHandler
{
    private readonly IApplicationDbContext _context;

    public GetPassportTravelRecordsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<TravelRecordDto>>> GetTravelRecordsByPassportNumberAsync(
        string passportNumber,
        CancellationToken cancellationToken = default)
    {
        var passport = await _context.Passports
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PassportNumber == passportNumber, cancellationToken);

        if (passport == null)
        {
            return ApiResponse<List<TravelRecordDto>>.Failure(
                $"Passport with number '{passportNumber}' was not found.", statusCode: 404);
        }

        var records = await _context.TravelRecords
            .AsNoTracking()
            .Where(t => t.PassportId == passport.Id)
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
