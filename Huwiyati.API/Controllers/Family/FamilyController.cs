namespace Huwiyati.API.Controllers.Family;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Huwiyati.Application.Family.Commands;
using Huwiyati.Application.Family.Queries;

[ApiController]
[Route("api/v1/families")]
public class FamilyController : ControllerBase
{
    private readonly CreateFamilyCardHandler _createFamilyCardHandler;
    private readonly RenewFamilyCardHandler _renewFamilyCardHandler;
    private readonly AddWifeHandler _addWifeHandler;
    private readonly UpdateFamilyMemberStatusHandler _updateFamilyMemberStatusHandler;
    private readonly GetActiveFamiliesHandler _getActiveFamiliesHandler;
    private readonly GetFamilyByIdHandler _getFamilyByIdHandler;
    private readonly GetFamilyHistoryByFamilyNumberHandler _getFamilyHistoryByFamilyNumberHandler;

    public FamilyController(
        CreateFamilyCardHandler createFamilyCardHandler,
        RenewFamilyCardHandler renewFamilyCardHandler,
        AddWifeHandler addWifeHandler,
        UpdateFamilyMemberStatusHandler updateFamilyMemberStatusHandler,
        GetActiveFamiliesHandler getActiveFamiliesHandler,
        GetFamilyByIdHandler getFamilyByIdHandler,
        GetFamilyHistoryByFamilyNumberHandler getFamilyHistoryByFamilyNumberHandler)
    {
        _createFamilyCardHandler = createFamilyCardHandler;
        _renewFamilyCardHandler = renewFamilyCardHandler;
        _addWifeHandler = addWifeHandler;
        _updateFamilyMemberStatusHandler = updateFamilyMemberStatusHandler;
        _getActiveFamiliesHandler = getActiveFamiliesHandler;
        _getFamilyByIdHandler = getFamilyByIdHandler;
        _getFamilyHistoryByFamilyNumberHandler = getFamilyHistoryByFamilyNumberHandler;
    }

    /// <summary>
    /// Create a new Family Record, Marriage Contract, and issue initial Family Card
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateFamilyCard(
        [FromBody] CreateFamilyCardCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _createFamilyCardHandler.CreateAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Renew an existing Family Card (Allowed within 90 days of expiry or if expired)
    /// </summary>
    [HttpPost("renew")]
    public async Task<IActionResult> RenewFamilyCard(
        [FromBody] RenewFamilyCardCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _renewFamilyCardHandler.RenewAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Register an additional wife and marriage contract to an existing active Family Card using Husband's National Number
    /// </summary>
    [HttpPost("add-wife")]
    public async Task<IActionResult> AddWife(
        [FromBody] AddWifeCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _addWifeHandler.AddWifeAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update status of a family member (Divorced, Deceased, Left, Active) and automatically update linked marriage contract / civil status
    /// </summary>
    [HttpPut("members/status")]
    public async Task<IActionResult> UpdateMemberStatus(
        [FromBody] UpdateFamilyMemberStatusCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _updateFamilyMemberStatusHandler.UpdateStatusAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get all active Family Card records in summary view
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetActiveFamilies(CancellationToken cancellationToken)
    {
        var result = await _getActiveFamiliesHandler.GetActiveFamiliesAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get complete Family Card record details by Family Card ID (including full members list)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFamilyById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getFamilyByIdHandler.GetFamilyByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get complete family card history (Active & Expired renewals) by 11-digit Family Number
    /// </summary>
    [HttpGet("history/{familyNumber}")]
    public async Task<IActionResult> GetFamilyHistoryByFamilyNumber(string familyNumber, CancellationToken cancellationToken)
    {
        var result = await _getFamilyHistoryByFamilyNumberHandler.GetHistoryAsync(familyNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}