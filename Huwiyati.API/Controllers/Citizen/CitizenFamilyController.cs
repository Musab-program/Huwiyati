namespace Huwiyati.API.Controllers.Citizen;

using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Family.Queries;
using Huwiyati.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Provides citizen portal endpoints for querying family cards and household records.
/// </summary>
[ApiController]
[Route("api/v1/citizen/family")]
[Authorize(Roles = AppRoles.Citizen)]
public class CitizenFamilyController : ControllerBase
{
    private readonly GetCitizenFamilyCardsHandler _getCitizenFamilyCardsHandler;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CitizenFamilyController"/> class.
    /// </summary>
    public CitizenFamilyController(
        GetCitizenFamilyCardsHandler getCitizenFamilyCardsHandler,
        IIdentityService identityService)
    {
        _getCitizenFamilyCardsHandler = getCitizenFamilyCardsHandler;
        _identityService = identityService;
    }

    /// <summary>
    /// Retrieves active and associated family card records for the authenticated citizen
    /// </summary>
    [HttpGet("my-family-cards")]
    public async Task<IActionResult> GetMyFamilyCards(CancellationToken cancellationToken)
    {
        var personId = await _identityService.GetPersonIdFromClaimsAsync(User, cancellationToken);
        if (personId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Citizen security context invalid.", statusCode: 401));
        }

        var result = await _getCitizenFamilyCardsHandler.GetCitizenFamiliesAsync(personId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
