namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;

/// <summary>
/// Handles querying lifetime border travel history for a citizen across all past and active passports.
/// </summary>
public class GetPersonTravelHistoryHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonTravelHistoryHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetPersonTravelHistoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all travel records associated with the citizen identified by national number.
    /// </summary>
    /// <param name="nationalNumber">The 11-digit national number of the citizen.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of <see cref="TravelRecordDto"/>.</returns>
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
