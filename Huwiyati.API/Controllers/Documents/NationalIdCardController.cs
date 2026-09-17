namespace Huwiyati.API.Controllers.Documents;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Huwiyati.Application.Documents.NationalIdCard.Commands;
using Huwiyati.Application.Documents.NationalIdCard.Queries;

[ApiController]
[Route("api/v1/national-id-cards")]
public class NationalIdCardController : ControllerBase
{
    private readonly IssueNationalIdCardHandler _issueNationalIdCardHandler;
    private readonly RenewNationalIdCardHandler _renewNationalIdCardHandler;
    private readonly UpdatePersonDataHandler _updatePersonDataHandler;
    private readonly GetNationalIdCardsHandler _getNationalIdCardsHandler;
    private readonly GetNationalIdCardByIdHandler _getNationalIdCardByIdHandler;
    private readonly GetPersonNationalIdCardHistoryHandler _getPersonNationalIdCardHistoryHandler;

    public NationalIdCardController(
        IssueNationalIdCardHandler issueNationalIdCardHandler,
        RenewNationalIdCardHandler renewNationalIdCardHandler,
        UpdatePersonDataHandler updatePersonDataHandler,
        GetNationalIdCardsHandler getNationalIdCardsHandler,
        GetNationalIdCardByIdHandler getNationalIdCardByIdHandler,
        GetPersonNationalIdCardHistoryHandler getPersonNationalIdCardHistoryHandler)
    {
        _issueNationalIdCardHandler = issueNationalIdCardHandler;
        _renewNationalIdCardHandler = renewNationalIdCardHandler;
        _updatePersonDataHandler = updatePersonDataHandler;
        _getNationalIdCardsHandler = getNationalIdCardsHandler;
        _getNationalIdCardByIdHandler = getNationalIdCardByIdHandler;
        _getPersonNationalIdCardHistoryHandler = getPersonNationalIdCardHistoryHandler;
    }

    /// <summary>
    /// Issue a new National ID Card for an existing or newly registered Person
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> IssueNationalIdCard(
        [FromBody] IssueNationalIdCardCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _issueNationalIdCardHandler.IssueAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Renew an existing National ID Card for a citizen (Deactivates old active card if within 90 days of expiry and issues new 10-year card)
    /// </summary>
    [HttpPost("renew")]
    public async Task<IActionResult> RenewNationalIdCard(
        [FromBody] RenewNationalIdCardCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _renewNationalIdCardHandler.RenewAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update existing citizen details linked to an active National ID Card
    /// </summary>
    [HttpPut("person-data")]
    public async Task<IActionResult> UpdatePersonData(
        [FromBody] UpdatePersonDataCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _updatePersonDataHandler.UpdateAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get all active National ID Cards in the system
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetActiveNationalIdCards(CancellationToken cancellationToken)
    {
        var result = await _getNationalIdCardsHandler.GetActiveNationalIdCardsAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get active National ID Card details by Card ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNationalIdCardById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getNationalIdCardByIdHandler.GetNationalIdCardByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get complete card history (Active, Expired, Lost, Suspended) for a citizen by National Number
    /// </summary>
    [HttpGet("history/{nationalNumber}")]
    public async Task<IActionResult> GetCardHistoryByNationalNumber(string nationalNumber, CancellationToken cancellationToken)
    {
        var result = await _getPersonNationalIdCardHistoryHandler.GetCardHistoryByNationalNumberAsync(nationalNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
