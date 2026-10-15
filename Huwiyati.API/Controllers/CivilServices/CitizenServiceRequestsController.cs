namespace Huwiyati.API.Controllers.CivilServices;

using System.Security.Claims;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Requests.Commands;
using Huwiyati.Application.Requests.Queries;
using Huwiyati.Domain.Constants;
using Huwiyati.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Provides citizen portal endpoints for creating, cancelling, and viewing civil service requests.
/// </summary>
[ApiController]
[Route("api/v1/citizen/service-requests")]
[Authorize(Roles = AppRoles.Citizen)]
public class CitizenServiceRequestsController : ControllerBase
{
    private readonly CreateServiceRequestHandler _createHandler;
    private readonly CancelServiceRequestHandler _cancelHandler;
    private readonly GetCitizenServiceRequestsHandler _getCitizenRequestsHandler;
    private readonly GetServiceRequestByIdHandler _getByIdHandler;
    private readonly GetServiceTypesHandler _getServiceTypesHandler;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CitizenServiceRequestsController"/> class.
    /// </summary>
    public CitizenServiceRequestsController(
        CreateServiceRequestHandler createHandler,
        CancelServiceRequestHandler cancelHandler,
        GetCitizenServiceRequestsHandler getCitizenRequestsHandler,
        GetServiceRequestByIdHandler getByIdHandler,
        GetServiceTypesHandler getServiceTypesHandler,
        UserManager<ApplicationUser> userManager,
        IIdentityService identityService)
    {
        _createHandler = createHandler;
        _cancelHandler = cancelHandler;
        _getCitizenRequestsHandler = getCitizenRequestsHandler;
        _getByIdHandler = getByIdHandler;
        _getServiceTypesHandler = getServiceTypesHandler;
        _userManager = userManager;
        _identityService = identityService;
    }

    /// <summary>
    /// Creates a new service request on behalf of the authenticated citizen.
    /// </summary>
    /// <param name="command">Request creation payload including service type, branch, and JSON form data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response containing the created request details.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromBody] CreateServiceRequestCommand command, CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserIdFromClaims(User);
        if (userId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID in token.", statusCode: 401));
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.PersonId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("User account or associated Person record not found.", statusCode: 401));
        }

        var result = await _createHandler.CreateAsync(command, user.PersonId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Cancels a pending service request owned by the authenticated citizen.
    /// </summary>
    /// <param name="command">Cancellation command containing request ID and cancellation reason.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response confirming cancellation.</returns>
    [HttpPost("cancel")]
    public async Task<IActionResult> CancelRequest([FromBody] CancelServiceRequestCommand command, CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserIdFromClaims(User);
        if (userId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID in token.", statusCode: 401));
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.PersonId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("User account or associated Person record not found.", statusCode: 401));
        }

        var result = await _cancelHandler.CancelAsync(command, currentPersonId: user.PersonId, currentEmployeeUserId: null, cancellationToken: cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves a paginated list of service requests submitted by the authenticated citizen.
    /// </summary>
    /// <param name="query">Pagination and status filter query parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of citizen service requests.</returns>
    [HttpGet]
    public async Task<IActionResult> GetMyRequests([FromQuery] GetCitizenServiceRequestsQuery query, CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserIdFromClaims(User);
        if (userId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID in token.", statusCode: 401));
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.PersonId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("User account or associated Person record not found.", statusCode: 401));
        }

        var result = await _getCitizenRequestsHandler.GetCitizenRequestsAsync(query, user.PersonId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves complete details of a specific service request owned by the authenticated citizen.
    /// </summary>
    /// <param name="id">The unique identifier of the service request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response containing service request details.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRequestById(Guid id, CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserIdFromClaims(User);
        if (userId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID in token.", statusCode: 401));
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.PersonId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("User account or associated Person record not found.", statusCode: 401));
        }

        var result = await _getByIdHandler.GetByIdAsync(id, currentPersonId: user.PersonId, currentEmployeeUserId: null, cancellationToken: cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves all available service types, optionally filtered by organization.
    /// </summary>
    /// <param name="organizationId">Optional organization ID filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response containing the list of available service types.</returns>
    [HttpGet("service-types")]
    public async Task<IActionResult> GetServiceTypes([FromQuery] Guid? organizationId, CancellationToken cancellationToken)
    {
        var query = new GetServiceTypesQuery(organizationId);
        var result = await _getServiceTypesHandler.GetServiceTypesAsync(query, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
