namespace Huwiyati.Application.Documents.Passport.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;

/// <summary>
/// Handles querying single travel record details by its unique identifier.
/// </summary>
public class GetTravelRecordByIdHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTravelRecordByIdHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetTravelRecordByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves travel record details projected into <see cref="TravelRecordDto"/> by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the travel record.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the matching <see cref="TravelRecordDto"/> or a 404 response.</returns>
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
