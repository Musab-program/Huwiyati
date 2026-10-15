namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;

/// <summary>
/// Handles querying all border and travel movements associated with a specific passport number.
/// </summary>
public class GetPassportTravelRecordsHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPassportTravelRecordsHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetPassportTravelRecordsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all travel movement records recorded under the given passport number.
    /// </summary>
    /// <param name="passportNumber">The official passport number.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of <see cref="TravelRecordDto"/>.</returns>
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
