namespace Huwiyati.API.Controllers.Citizen;

using Huwiyati.Application.Authentication.Queries;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Provides citizen portal endpoints for querying account profile and civil registry metadata.
/// </summary>
[ApiController]
[Route("api/v1/citizen/profile")]
[Authorize(Roles = AppRoles.Citizen)]
public class CitizenProfileController : ControllerBase
{
    private readonly GetCitizenProfileHandler _getCitizenProfileHandler;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CitizenProfileController"/> class.
    /// </summary>
    public CitizenProfileController(
        GetCitizenProfileHandler getCitizenProfileHandler,
        IIdentityService identityService)
    {
        _getCitizenProfileHandler = getCitizenProfileHandler;
        _identityService = identityService;
    }

    /// <summary>
    /// Retrieves detailed account profile and civil registry metadata for the authenticated citizen
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserIdFromClaims(User);
        if (userId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Citizen security context invalid.", statusCode: 401));
        }

        var result = await _getCitizenProfileHandler.GetProfileAsync(userId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
