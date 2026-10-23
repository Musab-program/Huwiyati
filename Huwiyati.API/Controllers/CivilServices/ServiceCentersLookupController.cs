namespace Huwiyati.API.Controllers.CivilServices;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Huwiyati.Domain.Constants;
using Huwiyati.Application.Organizations.Queries;
using Huwiyati.Application.Organizations.Branches.Queries;

/// <summary>
/// Provides read-only directory lookup endpoints for citizens, employees, and administrators
/// to query active government organizations and active service delivery centers/branches.
/// </summary>
[ApiController]
[Route("api/v1/service-centers-lookup")]
[Authorize(Roles = $"{AppRoles.Citizen},{AppRoles.Employee},{AppRoles.Admin},{AppRoles.SuperAdmin}")]
public class ServiceCentersLookupController : ControllerBase
{
    private readonly GetOrganizationsQueryHandler _getOrganizationsQueryHandler;
    private readonly GetActiveBranchesByOrganizationHandler _getActiveBranchesHandler;
    private readonly GetActiveBranchByIdHandler _getActiveBranchByIdHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceCentersLookupController"/> class.
    /// </summary>
    /// <param name="getOrganizationsQueryHandler">The organization query handler instance.</param>
    /// <param name="getActiveBranchesHandler">The active branches query handler instance.</param>
    /// <param name="getActiveBranchByIdHandler">The active branch details query handler instance.</param>
    public ServiceCentersLookupController(
        GetOrganizationsQueryHandler getOrganizationsQueryHandler,
        GetActiveBranchesByOrganizationHandler getActiveBranchesHandler,
        GetActiveBranchByIdHandler getActiveBranchByIdHandler)
    {
        _getOrganizationsQueryHandler = getOrganizationsQueryHandler;
        _getActiveBranchesHandler = getActiveBranchesHandler;
        _getActiveBranchByIdHandler = getActiveBranchByIdHandler;
    }

    /// <summary>
    /// Retrieves all active government organizations available for citizen service selection.
    /// Route: GET api/v1/service-centers-lookup/organizations
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the list of active organizations.</returns>
    [HttpGet("organizations")]
    public async Task<IActionResult> GetActiveOrganizations(CancellationToken cancellationToken)
    {
        var result = await _getOrganizationsQueryHandler.GetOrganizationsAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves all active service delivery centers/branches belonging to a specified organization.
    /// The organization identifier is strictly required to prevent cross-service boundary confusion.
    /// Route: GET api/v1/service-centers-lookup?organizationId={guid}
    /// </summary>
    /// <param name="organizationId">The required unique identifier of the target organization.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the list of active service centers for the organization.</returns>
    [HttpGet]
    public async Task<IActionResult> GetActiveBranches(
        [FromQuery, Required] Guid organizationId,
        CancellationToken cancellationToken)
    {
        var result = await _getActiveBranchesHandler.HandleAsync(organizationId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves details of an active service delivery center/branch by its unique identifier.
    /// Returns 404 if the branch does not exist or is currently inactive.
    /// Route: GET api/v1/service-centers-lookup/{id}
    /// </summary>
    /// <param name="id">The unique identifier of the service center.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with the active service center details if found; otherwise 404.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetActiveBranchById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getActiveBranchByIdHandler.HandleAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
