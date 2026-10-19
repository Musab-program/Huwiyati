namespace Huwiyati.API.Controllers.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Huwiyati.Domain.Constants;
using Huwiyati.Application.Authentication.Commands;
using Huwiyati.Application.Authentication.DTOs;
using Huwiyati.Application.Authentication.Queries;

/// <summary>
/// Handles administrative citizen account queries and management for authorized staff and SuperAdmin.
/// Previously part of AccountController.
/// </summary>
[ApiController]
[Route("api/v1/users-management")]
public class UserManagementController : ControllerBase
{
    private readonly GetCitizenAccountStatusHandler _getCitizenAccountStatusHandler;
    private readonly ActivateCitizenAccountHandler _activateCitizenAccountHandler;
    private readonly GetAllUserAccountsHandler _getAllUserAccountsHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserManagementController"/> class.
    /// </summary>
    public UserManagementController(
        GetCitizenAccountStatusHandler getCitizenAccountStatusHandler,
        ActivateCitizenAccountHandler activateCitizenAccountHandler,
        GetAllUserAccountsHandler getAllUserAccountsHandler)
    {
        _getCitizenAccountStatusHandler = getCitizenAccountStatusHandler;
        _activateCitizenAccountHandler = activateCitizenAccountHandler;
        _getAllUserAccountsHandler = getAllUserAccountsHandler;
    }

    /// <summary>
    /// Retrieves the registration and account activation status of a citizen by national number.
    /// Route: GET api/v1/users-management/citizen-status/{nationalNumber}
    /// </summary>
    /// <param name="nationalNumber">The citizen national number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with citizen account status details.</returns>
    [HttpGet("citizen-status/{nationalNumber}")]
    [Authorize(Roles = $"{AppRoles.Employee},{AppRoles.Admin}")]
    public async Task<IActionResult> GetCitizenAccountStatus(string nationalNumber, CancellationToken cancellationToken)
    {
        var result = await _getCitizenAccountStatusHandler.GetCitizenAccountStatusAsync(nationalNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Activates a citizen account manually by an authorized civil registry employee or administrator.
    /// Route: POST api/v1/users-management/activate-citizen
    /// </summary>
    /// <param name="request">Request containing citizen national number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response confirming account activation.</returns>
    [HttpPost("activate-citizen")]
    [Authorize(Roles = $"{AppRoles.Employee},{AppRoles.Admin}")]
    public async Task<IActionResult> ActivateCitizenAccount([FromBody] ActivateCitizenAccountRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _activateCitizenAccountHandler.ActivateAccountAsync(request.NationalNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves all registered user accounts across the platform. Accessible only by SuperAdmin.
    /// Route: GET api/v1/users-management/all-users
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response containing list of all registered user accounts.</returns>
    [HttpGet("all-users")]
    [Authorize(Roles = AppRoles.SuperAdmin)]
    public async Task<IActionResult> GetAllUserAccounts(CancellationToken cancellationToken)
    {
        var result = await _getAllUserAccountsHandler.GetAllUserAccountsAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
