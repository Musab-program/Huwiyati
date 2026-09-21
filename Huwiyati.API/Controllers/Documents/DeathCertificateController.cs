namespace Huwiyati.API.Controllers.Documents;

using Microsoft.AspNetCore.Mvc;
using Huwiyati.Application.Documents.DeathCertificate.Commands;
using Huwiyati.Application.Documents.DeathCertificate.Queries;

[ApiController]
[Route("api/v1/death-certificates")]
public class DeathCertificateController : ControllerBase
{
    private readonly IssueDeathCertificateHandler _issueDeathCertificateHandler;
    private readonly UpdateDeathCertificateHandler _updateDeathCertificateHandler;
    private readonly GetAllDeathCertificatesHandler _getAllDeathCertificatesHandler;
    private readonly GetDeathCertificateByIdHandler _getDeathCertificateByIdHandler;

    public DeathCertificateController(
        IssueDeathCertificateHandler issueDeathCertificateHandler,
        UpdateDeathCertificateHandler updateDeathCertificateHandler,
        GetAllDeathCertificatesHandler getAllDeathCertificatesHandler,
        GetDeathCertificateByIdHandler getDeathCertificateByIdHandler)
    {
        _issueDeathCertificateHandler = issueDeathCertificateHandler;
        _updateDeathCertificateHandler = updateDeathCertificateHandler;
        _getAllDeathCertificatesHandler = getAllDeathCertificatesHandler;
        _getDeathCertificateByIdHandler = getDeathCertificateByIdHandler;
    }

    /// <summary>
    /// Issue an official Death Certificate for a deceased person
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> IssueDeathCertificate(
        [FromBody] IssueDeathCertificateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _issueDeathCertificateHandler.IssueAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update details of an existing Death Certificate
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateDeathCertificate(
        [FromBody] UpdateDeathCertificateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _updateDeathCertificateHandler.UpdateAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get all registered Death Certificates
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllDeathCertificates(CancellationToken cancellationToken)
    {
        var result = await _getAllDeathCertificatesHandler.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get Death Certificate details by Death Certificate ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeathCertificateById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getDeathCertificateByIdHandler.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
