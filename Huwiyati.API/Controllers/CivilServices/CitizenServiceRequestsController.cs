namespace Huwiyati.API.Controllers.CivilServices;

using System.Security.Claims;
using Huwiyati.Application.Common;
using Huwiyati.Application.Requests.Commands;
using Huwiyati.Application.Requests.Queries;
using Huwiyati.Domain.Constants;
using Huwiyati.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

    public CitizenServiceRequestsController(
        CreateServiceRequestHandler createHandler,
        CancelServiceRequestHandler cancelHandler,
        GetCitizenServiceRequestsHandler getCitizenRequestsHandler,
        GetServiceRequestByIdHandler getByIdHandler,
        GetServiceTypesHandler getServiceTypesHandler,
        UserManager<ApplicationUser> userManager)
    {
        _createHandler = createHandler;
        _cancelHandler = cancelHandler;
        _getCitizenRequestsHandler = getCitizenRequestsHandler;
        _getByIdHandler = getByIdHandler;
        _getServiceTypesHandler = getServiceTypesHandler;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromBody] CreateServiceRequestCommand command, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
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

    [HttpPost("cancel")]
    public async Task<IActionResult> CancelRequest([FromBody] CancelServiceRequestCommand command, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
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

    [HttpGet]
    public async Task<IActionResult> GetMyRequests([FromQuery] GetCitizenServiceRequestsQuery query, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRequestById(Guid id, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
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

    [HttpGet("service-types")]
    public async Task<IActionResult> GetServiceTypes([FromQuery] Guid? organizationId, CancellationToken cancellationToken)
    {
        var query = new GetServiceTypesQuery(organizationId);
        var result = await _getServiceTypesHandler.GetServiceTypesAsync(query, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
