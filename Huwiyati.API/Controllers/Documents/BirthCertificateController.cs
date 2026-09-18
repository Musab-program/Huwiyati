namespace Huwiyati.API.Controllers.Documents;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Huwiyati.Application.Documents.BirthCertificate.Commands;
using Huwiyati.Application.Documents.BirthCertificate.Queries;

[ApiController]
[Route("api/v1/birth-certificates")]
public class BirthCertificateController : ControllerBase
{
    private readonly IssueBirthCertificateHandler _issueBirthCertificateHandler;
    private readonly UpdateChildDataHandler _updateChildDataHandler;
    private readonly GetBirthCertificateByIdHandler _getBirthCertificateByIdHandler;
    private readonly GetBirthCertificatesByFatherNationalNumberHandler _getBirthCertificatesByFatherNationalNumberHandler;
    private readonly GetAllBirthCertificatesHandler _getAllBirthCertificatesHandler;

    public BirthCertificateController(
        IssueBirthCertificateHandler issueBirthCertificateHandler,
        UpdateChildDataHandler updateChildDataHandler,
        GetBirthCertificateByIdHandler getBirthCertificateByIdHandler,
        GetBirthCertificatesByFatherNationalNumberHandler getBirthCertificatesByFatherNationalNumberHandler,
        GetAllBirthCertificatesHandler getAllBirthCertificatesHandler)
    {
        _issueBirthCertificateHandler = issueBirthCertificateHandler;
        _updateChildDataHandler = updateChildDataHandler;
        _getBirthCertificateByIdHandler = getBirthCertificateByIdHandler;
        _getBirthCertificatesByFatherNationalNumberHandler = getBirthCertificatesByFatherNationalNumberHandler;
        _getAllBirthCertificatesHandler = getAllBirthCertificatesHandler;
    }

    /// <summary>
    /// Issue a new official Birth Certificate for a newborn child and link to parents and family
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> IssueBirthCertificate(
        [FromBody] IssueBirthCertificateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _issueBirthCertificateHandler.IssueAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update personal details of a child linked to a Birth Certificate
    /// </summary>
    [HttpPut("child-data")]
    public async Task<IActionResult> UpdateChildData(
        [FromBody] UpdateChildDataCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _updateChildDataHandler.UpdateAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get all registered Birth Certificates in Civil Registry
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllBirthCertificates(CancellationToken cancellationToken)
    {
        var result = await _getAllBirthCertificatesHandler.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get Birth Certificate details by Birth Certificate ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBirthCertificateById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getBirthCertificateByIdHandler.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get all Birth Certificates issued for children of a father by Father National Number
    /// </summary>
    [HttpGet("father/{fatherNationalNumber}")]
    public async Task<IActionResult> GetBirthCertificatesByFatherNationalNumber(
        string fatherNationalNumber,
        CancellationToken cancellationToken)
    {
        var result = await _getBirthCertificatesByFatherNationalNumberHandler.GetByFatherNationalNumberAsync(fatherNationalNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
