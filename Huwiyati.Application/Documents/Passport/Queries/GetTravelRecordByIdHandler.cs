namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;

// Handler returning single Travel Record details by ID
public class GetTravelRecordByIdHandler
{
    private readonly IApplicationDbContext _context;

    public GetTravelRecordByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<TravelRecordDto>> GetTravelRecordByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var record = await _context.TravelRecords
            .AsNoTracking()
            .Where(t => t.Id == id)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (record == null)
        {
            return ApiResponse<TravelRecordDto>.Failure(
                "Travel record with the specified ID was not found.", statusCode: 404);
        }

        return ApiResponse<TravelRecordDto>.Success(record);
    }
}
