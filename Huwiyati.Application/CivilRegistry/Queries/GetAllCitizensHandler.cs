namespace Huwiyati.Application.CivilRegistry.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.CivilRegistry.DTOs;

// Query handler returning list of all citizens in Civil Registry for SuperAdmin
public class GetAllCitizensHandler
{
    private readonly IApplicationDbContext _context;

    public GetAllCitizensHandler(IApplicationDbContext context)
    {
        _context = context;
    }

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
