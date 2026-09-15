namespace Huwiyati.API.Controllers.Organizations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Huwiyati.Domain.Constants;
using Huwiyati.Application.Organizations.Admins.Commands;
using Huwiyati.Application.Organizations.Admins.Queries;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = AppRoles.SuperAdmin)]
public class AdminsController : ControllerBase
{
    private readonly AssignAdminHandler _assignAdminHandler;
    private readonly GetAdminsHandler _getAdminsHandler;
    private readonly GetAdminByIdHandler _getAdminByIdHandler;
    private readonly UpdateAdminHandler _updateAdminHandler;
    private readonly RemoveAdminHandler _removeAdminHandler;
    private readonly RestoreAdminHandler _restoreAdminHandler;

    public AdminsController(
        AssignAdminHandler assignAdminHandler,
        GetAdminsHandler getAdminsHandler,
        GetAdminByIdHandler getAdminByIdHandler,
        UpdateAdminHandler updateAdminHandler,
        RemoveAdminHandler removeAdminHandler,
        RestoreAdminHandler restoreAdminHandler)
    {
        _assignAdminHandler = assignAdminHandler;
        _getAdminsHandler = getAdminsHandler;
        _getAdminByIdHandler = getAdminByIdHandler;
        _updateAdminHandler = updateAdminHandler;
        _removeAdminHandler = removeAdminHandler;
        _restoreAdminHandler = restoreAdminHandler;
    }

    /// <summary>
    /// Get all admins across all organizations/branches (or filter by organizationId or branchId)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAdmins(
        [FromQuery] Guid? organizationId,
        [FromQuery] Guid? branchId,
        CancellationToken cancellationToken)
    {
        var result = await _getAdminsHandler.GetAdminsAsync(organizationId, branchId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get single admin details by Employee ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdminById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getAdminByIdHandler.GetAdminByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Assign an existing citizen as an Admin to an Organization Branch
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AssignAdmin([FromBody] AssignAdminCommand command, CancellationToken cancellationToken)
    {
        var result = await _assignAdminHandler.AssignAdminAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update admin branch or active status
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdmin(Guid id, [FromBody] UpdateAdminCommand command, CancellationToken cancellationToken)
    {
        var result = await _updateAdminHandler.UpdateAdminAsync(id, command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Remove Admin role (Relieve of admin duties, user remains a regular employee)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveAdmin(Guid id, CancellationToken cancellationToken)
    {
        var result = await _removeAdminHandler.RemoveAdminRoleAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Restore Admin role to a relieved employee
    /// </summary>
    [HttpPost("{id}/restore")]
    public async Task<IActionResult> RestoreAdmin(Guid id, CancellationToken cancellationToken)
    {
        var result = await _restoreAdminHandler.RestoreAdminRoleAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
