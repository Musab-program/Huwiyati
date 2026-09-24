namespace Huwiyati.API.Controllers.Documents;

using Microsoft.AspNetCore.Mvc;
using Huwiyati.Application.Documents.Passport.Commands;
using Huwiyati.Application.Documents.Passport.Queries;

[ApiController]
[Route("api/v1/passports")]
public class PassportController : ControllerBase
{
    private readonly IssuePassportHandler _issuePassportHandler;
    private readonly RenewPassportHandler _renewPassportHandler;
    private readonly GetActivePassportsHandler _getActivePassportsHandler;
    private readonly GetPassportByIdHandler _getPassportByIdHandler;
    private readonly GetPersonPassportHistoryHandler _getPersonPassportHistoryHandler;

    public PassportController(
        IssuePassportHandler issuePassportHandler,
        RenewPassportHandler renewPassportHandler,
        GetActivePassportsHandler getActivePassportsHandler,
        GetPassportByIdHandler getPassportByIdHandler,
        GetPersonPassportHistoryHandler getPersonPassportHistoryHandler)
    {
        _issuePassportHandler = issuePassportHandler;
        _renewPassportHandler = renewPassportHandler;
        _getActivePassportsHandler = getActivePassportsHandler;
        _getPassportByIdHandler = getPassportByIdHandler;
        _getPersonPassportHistoryHandler = getPersonPassportHistoryHandler;
    }

    /// <summary>
    /// Issue a new Passport for an existing Person
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> IssuePassport(
        [FromBody] IssuePassportCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _issuePassportHandler.IssueAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Renew an existing Passport for a citizen (Deactivates old active passport and issues a new 6-year passport)
    /// </summary>
    [HttpPost("renew")]
    public async Task<IActionResult> RenewPassport(
        [FromBody] RenewPassportCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _renewPassportHandler.RenewAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get all active Passports in the system
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetActivePassports(CancellationToken cancellationToken)
    {
        var result = await _getActivePassportsHandler.GetActivePassportsAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get active Passport details by Passport ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPassportById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getPassportByIdHandler.GetPassportByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get complete passport history (Active, Expired, Canceled) for a citizen by National Number
    /// </summary>
    [HttpGet("history/{nationalNumber}")]
    public async Task<IActionResult> GetPassportHistoryByNationalNumber(string nationalNumber, CancellationToken cancellationToken)
    {
        var result = await _getPersonPassportHistoryHandler.GetPassportHistoryByNationalNumberAsync(nationalNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}