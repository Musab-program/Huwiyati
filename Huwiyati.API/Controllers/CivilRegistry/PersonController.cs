namespace Huwiyati.API.Controllers.CivilRegistry;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Huwiyati.Domain.Constants;
using Huwiyati.Application.CivilRegistry.Queries;

/// <summary>
/// Provides administrative endpoints for managing and querying citizen civil registry records.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = AppRoles.SuperAdmin)]
public class PersonController : ControllerBase
{
    private readonly GetAllCitizensHandler _getAllCitizensHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonController"/> class.
    /// </summary>
    /// <param name="getAllCitizensHandler">Handler for querying all registered citizens.</param>
    public PersonController(GetAllCitizensHandler getAllCitizensHandler)
    {
        _getAllCitizensHandler = getAllCitizensHandler;
    }

    /// <summary>
    /// Get all citizens registered in Civil Registry (SuperAdmin only)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllCitizens(CancellationToken cancellationToken)
    {
        var result = await _getAllCitizensHandler.GetAllCitizensAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
