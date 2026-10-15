namespace Huwiyati.Application.Authentication.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;

/// <summary>
/// Query handler returning detailed citizen account registration and activation status by national number for administrative staff.
/// </summary>
public class GetCitizenAccountStatusHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCitizenAccountStatusHandler"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="identityService">The identity management service.</param>
    public GetCitizenAccountStatusHandler(
        IApplicationDbContext context,
        IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Retrieves account and registration status for a citizen using their national identification number.
    /// </summary>
    /// <param name="nationalNumber">The national number to query.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the citizen account status DTO.</returns>
    public async Task<ApiResponse<CitizenAccountStatusDto>> GetCitizenAccountStatusAsync(
        string nationalNumber,
        CancellationToken cancellationToken = default)
    {
        // 1. Check if Person exists in Civil Registry
        var person = await _context.Persons
            .AsNoTracking()
            .Where(p => p.NationalNumber == nationalNumber)
            .Select(p => new
            {
                p.Id,
                FullName = $"{p.FirstName} {p.FatherName} {p.GrandfatherName} {p.FamilyName}".Trim(),
                p.NationalNumber
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (person == null)
        {
            return ApiResponse<CitizenAccountStatusDto>.Failure(
                "No person record found in Civil Registry with the provided National Number.",
                statusCode: 404);
        }

        // 2. Check if User Account exists for this Person via Identity Service
        var userDetails = await _identityService.GetUserAccountDetailsByPersonIdAsync(person.Id, cancellationToken);

        var dto = new CitizenAccountStatusDto
        {
            PersonId = person.Id,
            PersonFullName = person.FullName,
            NationalNumber = person.NationalNumber,
            HasRegisteredAccount = userDetails != null,
            UserId = userDetails?.UserId,
            Email = userDetails?.Email,
            PhoneNumber = userDetails?.PhoneNumber,
            Status = userDetails?.Status,
            CreatedAt = userDetails?.CreatedAt,
            ActivatedAt = userDetails?.ActivatedAt
        };

        return ApiResponse<CitizenAccountStatusDto>.Success(dto);
    }
}
