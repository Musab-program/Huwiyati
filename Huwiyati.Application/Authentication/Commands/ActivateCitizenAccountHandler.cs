namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;
using Huwiyati.Domain.Enums;

// Command handler to activate a citizen's account by National Number
public class ActivateCitizenAccountHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public ActivateCitizenAccountHandler(
        IApplicationDbContext context,
        IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<ApiResponse<CitizenAccountStatusDto>> ActivateAccountAsync(
        string nationalNumber,
        CancellationToken cancellationToken = default)
    {
        // 1. Find Person in Civil Registry
        var person = await _context.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.NationalNumber == nationalNumber, cancellationToken);

        if (person == null)
        {
            return ApiResponse<CitizenAccountStatusDto>.Failure(
                "No person record found in Civil Registry with this National Number.",
                statusCode: 404);
        }

        // 2. Find User Account for Person via Identity Service
        var userDetails = await _identityService.GetUserAccountDetailsByPersonIdAsync(person.Id, cancellationToken);

        if (userDetails == null)
        {
            return ApiResponse<CitizenAccountStatusDto>.Failure(
                "The citizen has not registered an online account in Huwiyati app yet.",
                statusCode: 400);
        }

        // 3. Verify current account status
        if (userDetails.Status == AccountStatus.Active)
        {
            return ApiResponse<CitizenAccountStatusDto>.Failure(
                "Citizen account is already active.",
                statusCode: 400);
        }

        if (userDetails.Status == AccountStatus.Suspended || userDetails.Status == AccountStatus.Deactivated)
        {
            return ApiResponse<CitizenAccountStatusDto>.Failure(
                $"Cannot activate account because its status is '{userDetails.Status}'.",
                statusCode: 400);
        }

        // 4. Update status to Active via Identity Service
        var isUpdated = await _identityService.ChangeAccountStatusAsync(userDetails.UserId, AccountStatus.Active, cancellationToken);

        if (!isUpdated)
        {
            return ApiResponse<CitizenAccountStatusDto>.Failure(
                "Failed to update citizen account status.",
                statusCode: 500);
        }

        var fullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim();

        var resultDto = new CitizenAccountStatusDto
        {
            PersonId = person.Id,
            PersonFullName = fullName,
            NationalNumber = person.NationalNumber,
            HasRegisteredAccount = true,
            UserId = userDetails.UserId,
            Email = userDetails.Email,
            PhoneNumber = userDetails.PhoneNumber,
            Status = AccountStatus.Active,
            CreatedAt = userDetails.CreatedAt,
            ActivatedAt = DateTime.UtcNow
        };

        return ApiResponse<CitizenAccountStatusDto>.Success(resultDto, message: "Citizen account has been successfully activated.");
    }
}
