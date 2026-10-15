namespace Huwiyati.API.Controllers.CivilServices;

using System.Security.Claims;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Requests.Commands;
using Huwiyati.Application.Requests.Queries;
using Huwiyati.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Provides administrative and employee endpoints for processing, reviewing, approving, rejecting, and tracking branch service requests.
/// </summary>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeServiceRequestsController"/> class.
    /// </summary>
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

    /// <summary>
    /// Creates a service request on behalf of a citizen directly by an employee.
    /// </summary>
    /// <param name="command">Service request creation command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with created request details.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateRequestOnBehalf([FromBody] CreateServiceRequestCommand command, CancellationToken cancellationToken)
    {
        var result = await _createHandler.CreateAsync(command, currentPersonId: null, cancellationToken: cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Updates the review/fulfillment status of a service request and appends an audit status history note.
    /// </summary>
    /// <param name="command">Status update command containing new status and note.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with updated request information.</returns>
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

    /// <summary>
    /// Cancels a service request administratively on behalf of the branch authority.
    /// </summary>
    /// <param name="command">Cancellation payload including reason.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response confirming cancellation.</returns>
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

    /// <summary>
    /// Retrieves a paginated list of service requests assigned to the logged-in employee's branch.
    /// </summary>
    /// <param name="query">Pagination and status filter parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of service requests for the branch.</returns>
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

    /// <summary>
    /// Retrieves full details of a specific service request assigned to the employee's branch.
    /// </summary>
    /// <param name="id">The unique identifier of the service request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with detailed service request information.</returns>
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

    /// <summary>
    /// Retrieves available service types for the employee's administrative organization.
    /// </summary>
    /// <param name="organizationId">Optional organization ID filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the list of service types.</returns>
    [HttpGet("service-types")]
    public async Task<IActionResult> GetServiceTypes([FromQuery] Guid? organizationId, CancellationToken cancellationToken)
    {
        var query = new GetServiceTypesQuery(organizationId);
        var result = await _getServiceTypesHandler.GetServiceTypesAsync(query, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
