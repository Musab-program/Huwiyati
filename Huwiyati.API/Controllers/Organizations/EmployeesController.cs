namespace Huwiyati.API.Controllers.Organizations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Constants;
using Huwiyati.Application.Organizations.Employees.Commands;
using Huwiyati.Application.Organizations.Employees.Queries;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
public class EmployeesController : ControllerBase
{
    private readonly AssignEmployeeHandler _assignEmployeeHandler;
    private readonly UpdateEmployeeHandler _updateEmployeeHandler;
    private readonly GetEmployeesHandler _getEmployeesHandler;
    private readonly GetEmployeeByIdHandler _getEmployeeByIdHandler;
    private readonly DeactivateEmployeeHandler _deactivateEmployeeHandler;
    private readonly ActivateEmployeeHandler _activateEmployeeHandler;
    private readonly IIdentityService _identityService;

    public EmployeesController(
        AssignEmployeeHandler assignEmployeeHandler,
        UpdateEmployeeHandler updateEmployeeHandler,
        GetEmployeesHandler getEmployeesHandler,
        GetEmployeeByIdHandler getEmployeeByIdHandler,
        DeactivateEmployeeHandler deactivateEmployeeHandler,
        ActivateEmployeeHandler activateEmployeeHandler,
        IIdentityService identityService)
    {
        _assignEmployeeHandler = assignEmployeeHandler;
        _updateEmployeeHandler = updateEmployeeHandler;
        _getEmployeesHandler = getEmployeesHandler;
        _getEmployeeByIdHandler = getEmployeeByIdHandler;
        _deactivateEmployeeHandler = deactivateEmployeeHandler;
        _activateEmployeeHandler = activateEmployeeHandler;
        _identityService = identityService;
    }

    /// <summary>
    /// Get all employees in current admin's branch
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetEmployees(CancellationToken cancellationToken)
    {
        var adminUserId = _identityService.GetUserIdFromClaims(User);
        if (adminUserId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID claim in token.", statusCode: 401));
        }

        var result = await _getEmployeesHandler.GetEmployeesAsync(adminUserId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get single employee details by Employee ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEmployeeById(Guid id, CancellationToken cancellationToken)
    {
        var adminUserId = _identityService.GetUserIdFromClaims(User);
        if (adminUserId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID claim in token.", statusCode: 401));
        }

        var result = await _getEmployeeByIdHandler.GetEmployeeByIdAsync(adminUserId, id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Assign an existing citizen as an Employee to the current Admin's Branch
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AssignEmployee([FromBody] AssignEmployeeCommand command, CancellationToken cancellationToken)
    {
        var adminUserId = _identityService.GetUserIdFromClaims(User);
        if (adminUserId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID claim in token.", statusCode: 401));
        }

        var result = await _assignEmployeeHandler.AssignEmployeeAsync(adminUserId, command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update employee branch (transfers employee to a new branch and generates a new EmployeeNumber)
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var adminUserId = _identityService.GetUserIdFromClaims(User);
        if (adminUserId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID claim in token.", statusCode: 401));
        }

        command.EmployeeId = id;
        var result = await _updateEmployeeHandler.UpdateEmployeeAsync(adminUserId, command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Deactivate employee in current admin's branch
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeactivateEmployee(Guid id, CancellationToken cancellationToken)
    {
        var adminUserId = _identityService.GetUserIdFromClaims(User);
        if (adminUserId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID claim in token.", statusCode: 401));
        }

        var result = await _deactivateEmployeeHandler.DeactivateEmployeeAsync(adminUserId, id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Activate employee in current admin's branch
    /// </summary>
    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> ActivateEmployee(Guid id, CancellationToken cancellationToken)
    {
        var adminUserId = _identityService.GetUserIdFromClaims(User);
        if (adminUserId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Invalid or missing User ID claim in token.", statusCode: 401));
        }

        var result = await _activateEmployeeHandler.ActivateEmployeeAsync(adminUserId, id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
