namespace Huwiyati.API.Controllers.Documents;

using Microsoft.AspNetCore.Mvc;
using Huwiyati.Application.Documents.Passport.Commands;
using Huwiyati.Application.Documents.Passport.Queries;

[ApiController]
[Route("api/v1/travel-records")]
public class TravelRecordController : ControllerBase
{
    private readonly AddTravelRecordHandler _addTravelRecordHandler;
    private readonly GetPassportTravelRecordsHandler _getPassportTravelRecordsHandler;
    private readonly GetPersonTravelHistoryHandler _getPersonTravelHistoryHandler;
    private readonly GetTravelRecordByIdHandler _getTravelRecordByIdHandler;

    public TravelRecordController(
        AddTravelRecordHandler addTravelRecordHandler,
        GetPassportTravelRecordsHandler getPassportTravelRecordsHandler,
        GetPersonTravelHistoryHandler getPersonTravelHistoryHandler,
        GetTravelRecordByIdHandler getTravelRecordByIdHandler)
    {
        _addTravelRecordHandler = addTravelRecordHandler;
        _getPassportTravelRecordsHandler = getPassportTravelRecordsHandler;
        _getPersonTravelHistoryHandler = getPersonTravelHistoryHandler;
        _getTravelRecordByIdHandler = getTravelRecordByIdHandler;
    }

    /// <summary>
    /// Add a new travel movement record for an active Passport
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddTravelRecord(
        [FromBody] AddTravelRecordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _addTravelRecordHandler.AddAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get a specific travel record by its unique ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTravelRecordById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getTravelRecordByIdHandler.GetTravelRecordByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get all travel records linked to a specific Passport Number
    /// </summary>
    [HttpGet("passport/{passportNumber}")]
    public async Task<IActionResult> GetTravelRecordsByPassportNumber(string passportNumber, CancellationToken cancellationToken)
    {
        var result = await _getPassportTravelRecordsHandler.GetTravelRecordsByPassportNumberAsync(passportNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get full lifetime travel history for a citizen by National Number across all passports
    /// </summary>
    [HttpGet("person/{nationalNumber}")]
    public async Task<IActionResult> GetPersonTravelHistoryByNationalNumber(string nationalNumber, CancellationToken cancellationToken)
    {
        var result = await _getPersonTravelHistoryHandler.GetTravelHistoryByNationalNumberAsync(nationalNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
