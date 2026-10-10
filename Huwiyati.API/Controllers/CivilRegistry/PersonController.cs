namespace Huwiyati.API.Controllers.CivilRegistry;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Huwiyati.Domain.Constants;
using Huwiyati.Application.CivilRegistry.Queries;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = AppRoles.SuperAdmin)]
public class PersonController : ControllerBase
{
    private readonly GetAllCitizensHandler _getAllCitizensHandler;

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
