namespace Huwiyati.Application.CivilRegistry.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.CivilRegistry.DTOs;

/// <summary>
/// Query handler returning the complete registry of all citizens registered in the Civil Registry authority.
/// </summary>
public class GetAllCitizensHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllCitizensHandler"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    public GetAllCitizensHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all citizen records from the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response containing the list of citizen DTOs.</returns>
    public async Task<ApiResponse<List<CivilRegistryPersonDto>>> GetAllCitizensAsync(
        CancellationToken cancellationToken = default)
    {
        var citizens = await _context.Persons
            .AsNoTracking()
            .OrderBy(p => p.CreatedAt)
            .Select(p => new CivilRegistryPersonDto
            {
                Id = p.Id,
                NationalNumber = p.NationalNumber,
                FullName = $"{p.FirstName} {p.FatherName} {p.GrandfatherName} {p.FamilyName}".Trim(),
                FirstName = p.FirstName,
                FatherName = p.FatherName,
                GrandfatherName = p.GrandfatherName,
                FamilyName = p.FamilyName,
                DateOfBirth = p.DateOfBirth,
                PlaceOfBirth = p.PlaceOfBirth,
                Gender = p.Gender,
                Nationality = p.Nationality,
                MaritalStatus = p.MaritalStatus,
                BloodGroup = p.BloodGroup,
                Governorate = p.Governorate,
                District = p.District,
                AddressDetails = p.AddressDetails,
                PhotoUrl = p.PhotoUrl,
                PersonStatus = p.PersonStatus
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<CivilRegistryPersonDto>>.Success(citizens);
    }
}
