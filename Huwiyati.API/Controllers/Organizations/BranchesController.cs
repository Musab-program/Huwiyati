namespace Huwiyati.API.Controllers.Organizations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Huwiyati.Domain.Constants;
using Huwiyati.Application.Organizations.Queries;
using Huwiyati.Application.Organizations.Branches.Commands;
using Huwiyati.Application.Organizations.Branches.Queries;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = AppRoles.SuperAdmin)]
public class BranchesController : ControllerBase
{
    private readonly CreateBranchHandler _createBranchHandler;
    private readonly UpdateBranchHandler _updateBranchHandler;
    private readonly DeleteBranchHandler _deleteBranchHandler;
    private readonly RestoreBranchHandler _restoreBranchHandler;
    private readonly GetOrganizationsQueryHandler _getOrganizationsQueryHandler;
    private readonly GetBranchesQueryHandler _getBranchesQueryHandler;
    private readonly GetBranchByIdHandler _getBranchByIdHandler;

    public BranchesController(
        CreateBranchHandler createBranchHandler,
        UpdateBranchHandler updateBranchHandler,
        DeleteBranchHandler deleteBranchHandler,
        RestoreBranchHandler restoreBranchHandler,
        GetOrganizationsQueryHandler getOrganizationsQueryHandler,
        GetBranchesQueryHandler getBranchesQueryHandler,
        GetBranchByIdHandler getBranchByIdHandler)
    {
        _createBranchHandler = createBranchHandler;
        _updateBranchHandler = updateBranchHandler;
        _deleteBranchHandler = deleteBranchHandler;
        _restoreBranchHandler = restoreBranchHandler;
        _getOrganizationsQueryHandler = getOrganizationsQueryHandler;
        _getBranchesQueryHandler = getBranchesQueryHandler;
        _getBranchByIdHandler = getBranchByIdHandler;
    }

    /// <summary>
    /// Get list of main organizations
    /// </summary>
    [HttpGet("organizations")]
    public async Task<IActionResult> GetOrganizations(CancellationToken cancellationToken)
    {
        var result = await _getOrganizationsQueryHandler.GetOrganizationsAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get all branches across all organizations (or filter by organizationId if provided)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetBranches([FromQuery] Guid? organizationId, CancellationToken cancellationToken)
    {
        var result = await _getBranchesQueryHandler.GetBranchesAsync(organizationId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get single branch details by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBranchById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getBranchByIdHandler.GetBranchByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Create a new organization branch
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchCommand command, CancellationToken cancellationToken)
    {
        var result = await _createBranchHandler.CreateBranchAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update existing organization branch details
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBranch(Guid id, [FromBody] UpdateBranchCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await _updateBranchHandler.UpdateBranchAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Delete organization branch (Soft delete: deactivates branch)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBranch(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteBranchHandler.DeleteBranchAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Restore organization branch (Reactivates soft-deleted branch)
    /// </summary>
    [HttpPost("{id}/restore")]
    public async Task<IActionResult> RestoreBranch(Guid id, CancellationToken cancellationToken)
    {
        var result = await _restoreBranchHandler.RestoreBranchAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}