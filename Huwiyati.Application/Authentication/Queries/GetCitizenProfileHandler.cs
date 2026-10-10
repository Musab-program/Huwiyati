namespace Huwiyati.Application.Authentication.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;
using Huwiyati.Domain.Enums;

public class GetCitizenProfileHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public GetCitizenProfileHandler(
        IApplicationDbContext context,
        IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<ApiResponse<CitizenProfileDto>> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            return ApiResponse<CitizenProfileDto>.Failure("Invalid user security identifier.", statusCode: 400);
        }

        var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(userId, cancellationToken);
        if (!contactInfo.HasValue || contactInfo.Value.PersonId == Guid.Empty)
        {
            return ApiResponse<CitizenProfileDto>.Failure("User account or associated Person record not found.", statusCode: 404);
        }

        var personId = contactInfo.Value.PersonId;

        var person = await _context.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == personId, cancellationToken);

        if (person == null)
        {
            return ApiResponse<CitizenProfileDto>.Failure("Citizen Civil Registry record not found.", statusCode: 404);
        }

        var hasActiveNationalId = await _context.NationalIdCards
            .AsNoTracking()
            .AnyAsync(c => c.PersonId == personId && c.Status == NationalIdCardStatus.Active, cancellationToken);

        var hasActivePassport = await _context.Passports
            .AsNoTracking()
            .AnyAsync(p => p.PersonId == personId && p.Status == PassportStatus.Active, cancellationToken);

        var hasActiveFamilyCard = await _context.Families
            .AsNoTracking()
            .AnyAsync(f => (f.HeadOfFamilyPersonId == personId || f.FamilyMembers.Any(m => m.PersonId == personId)) && f.Status == FamilyStatus.Active, cancellationToken);

        var fullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim();

        var profileDto = new CitizenProfileDto
        {
            UserId = userId,
            PersonId = person.Id,
            NationalNumber = person.NationalNumber,
            FullName = fullName,
            FirstName = person.FirstName,
            FatherName = person.FatherName,
            GrandfatherName = person.GrandfatherName,
            FamilyName = person.FamilyName,
            DateOfBirth = person.DateOfBirth,
            PlaceOfBirth = person.PlaceOfBirth,
            Gender = person.Gender.ToString(),
            Nationality = person.Nationality,
            MaritalStatus = person.MaritalStatus.ToString(),
            BloodGroup = person.BloodGroup.ToString(),
            Governorate = person.Governorate,
            District = person.District,
            AddressDetails = person.AddressDetails,
            PhotoUrl = person.PhotoUrl,
            Email = contactInfo.Value.Email,
            PhoneNumber = contactInfo.Value.PhoneNumber,
            AccountStatus = (await _identityService.IsUserActiveAsync(userId, cancellationToken)) ? "Active" : "PendingActivation",
            HasActiveNationalIdCard = hasActiveNationalId,
            HasActivePassport = hasActivePassport,
            HasActiveFamilyCard = hasActiveFamilyCard
        };

        return ApiResponse<CitizenProfileDto>.Success(profileDto, message: "Citizen profile details retrieved successfully.", statusCode: 200);
    }
}
