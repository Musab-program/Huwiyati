namespace Huwiyati.API.Controllers.CivilServices;

using System.Security.Claims;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Requests.Commands;
using Huwiyati.Application.Requests.Queries;
using Huwiyati.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/employee/service-requests")]
[Authorize(Roles = $"{AppRoles.Employee},{AppRoles.Admin}")]
public class EmployeeServiceRequestsController : ControllerBase
{
    private readonly CreateServiceRequestHandler _createHandler;
    private readonly ChangeServiceRequestStatusHandler _changeStatusHandler;
    private readonly CancelServiceRequestHandler _cancelHandler;
    private readonly GetBranchServiceRequestsHandler _getBranchRequestsHandler;
    private readonly GetServiceRequestByIdHandler _getByIdHandler;
    private readonly GetServiceTypesHandler _getServiceTypesHandler;
    private readonly IIdentityService _identityService;

    public EmployeeServiceRequestsController(
        CreateServiceRequestHandler createHandler,
        ChangeServiceRequestStatusHandler changeStatusHandler,
        CancelServiceRequestHandler cancelHandler,
        GetBranchServiceRequestsHandler getBranchRequestsHandler,
        GetServiceRequestByIdHandler getByIdHandler,
        GetServiceTypesHandler getServiceTypesHandler,
        IIdentityService identityService)
    {
        _createHandler = createHandler;
        _changeStatusHandler = changeStatusHandler;
        _cancelHandler = cancelHandler;
        _getBranchRequestsHandler = getBranchRequestsHandler;
        _getByIdHandler = getByIdHandler;
        _getServiceTypesHandler = getServiceTypesHandler;
        _identityService = identityService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequestOnBehalf([FromBody] CreateServiceRequestCommand command, CancellationToken cancellationToken)
    {
        var result = await _createHandler.CreateAsync(command, currentPersonId: null, cancellationToken: cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("change-status")]
    public async Task<IActionResult> ChangeRequestStatus([FromBody] ChangeServiceRequestStatusCommand command, CancellationToken cancellationToken)
    {
        var employeeUserId = _identityService.GetUserIdFromClaims(User);
        if (employeeUserId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID in token.", statusCode: 401));
        }

        var result = await _changeStatusHandler.ChangeStatusAsync(command, employeeUserId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> CancelRequestOnBehalf([FromBody] CancelServiceRequestCommand command, CancellationToken cancellationToken)
    {
        var employeeUserId = _identityService.GetUserIdFromClaims(User);
        if (employeeUserId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID in token.", statusCode: 401));
        }

        var result = await _cancelHandler.CancelAsync(command, currentPersonId: null, currentEmployeeUserId: employeeUserId, cancellationToken: cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetBranchRequests([FromQuery] GetBranchServiceRequestsQuery query, CancellationToken cancellationToken)
    {
        var employeeUserId = _identityService.GetUserIdFromClaims(User);
        if (employeeUserId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID in token.", statusCode: 401));
        }

        var result = await _getBranchRequestsHandler.GetBranchRequestsAsync(query, employeeUserId.ToString(), cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRequestById(Guid id, CancellationToken cancellationToken)
    {
        var employeeUserId = _identityService.GetUserIdFromClaims(User);
        if (employeeUserId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID in token.", statusCode: 401));
        }

        var result = await _getByIdHandler.GetByIdAsync(id, currentPersonId: null, currentEmployeeUserId: employeeUserId.ToString(), cancellationToken: cancellationToken);
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
