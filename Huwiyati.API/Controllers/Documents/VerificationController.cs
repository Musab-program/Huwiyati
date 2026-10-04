namespace Huwiyati.API.Controllers.Documents;

using Huwiyati.Application.Documents.Verification.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Public verification controller for scanning and verifying official documents via QR payload.
/// </summary>
[ApiController]
[Route("api/v1/verification")]
[AllowAnonymous]
public class VerificationController : ControllerBase
{
    private readonly VerifyDocumentByQrPayloadQueryHandler _verifyDocumentHandler;

    public VerificationController(VerifyDocumentByQrPayloadQueryHandler verifyDocumentHandler)
    {
        _verifyDocumentHandler = verifyDocumentHandler;
    }

    /// <summary>
    /// Step 1: Public HTTP GET endpoint to verify a document by its scanned QR payload or token
    /// </summary>
    /// <param name="payload">The scanned QR payload or token string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Verification result containing minimal necessary identity data.</returns>
    [HttpGet("{payload}")]
    public async Task<IActionResult> VerifyDocument(string payload, CancellationToken cancellationToken)
    {
        // Step 2: Delegate verification logic to the query handler
        var result = await _verifyDocumentHandler.VerifyAsync(payload, cancellationToken);

        // Step 3: Return appropriate HTTP response status code and result payload
        return StatusCode(result.StatusCode, result);
    }
}
