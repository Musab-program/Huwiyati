namespace Huwiyati.API.Controllers.Citizen;

using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.BirthCertificate.Queries;
using Huwiyati.Application.Documents.DeathCertificate.Queries;
using Huwiyati.Application.Documents.NationalIdCard.Queries;
using Huwiyati.Application.Documents.Passport.Queries;
using Huwiyati.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/citizen/documents")]
[Authorize(Roles = AppRoles.Citizen)]
public class CitizenDocumentsController : ControllerBase
{
    private readonly GetActiveNationalIdCardByPersonIdHandler _getActiveNationalIdCardHandler;
    private readonly GetActivePassportByPersonIdHandler _getActivePassportHandler;
    private readonly GetBirthCertificatesByFatherNationalNumberHandler _getBirthCertificatesByFatherHandler;
    private readonly GetBirthCertificateByIdHandler _getBirthCertificateByIdHandler;
    private readonly GetCitizenFamilyDeathCertificatesHandler _getFamilyDeathCertificatesHandler;
    private readonly GetPersonTravelHistoryHandler _getPersonTravelHistoryHandler;
    private readonly IIdentityService _identityService;

    public CitizenDocumentsController(
        GetActiveNationalIdCardByPersonIdHandler getActiveNationalIdCardHandler,
        GetActivePassportByPersonIdHandler getActivePassportHandler,
        GetBirthCertificatesByFatherNationalNumberHandler getBirthCertificatesByFatherHandler,
        GetBirthCertificateByIdHandler getBirthCertificateByIdHandler,
        GetCitizenFamilyDeathCertificatesHandler getFamilyDeathCertificatesHandler,
        GetPersonTravelHistoryHandler getPersonTravelHistoryHandler,
        IIdentityService identityService)
    {
        _getActiveNationalIdCardHandler = getActiveNationalIdCardHandler;
        _getActivePassportHandler = getActivePassportHandler;
        _getBirthCertificatesByFatherHandler = getBirthCertificatesByFatherHandler;
        _getBirthCertificateByIdHandler = getBirthCertificateByIdHandler;
        _getFamilyDeathCertificatesHandler = getFamilyDeathCertificatesHandler;
        _getPersonTravelHistoryHandler = getPersonTravelHistoryHandler;
        _identityService = identityService;
    }

    /// <summary>
    /// Retrieves active National ID Card details for the authenticated citizen
    /// </summary>
    [HttpGet("national-id-card")]
    public async Task<IActionResult> GetMyNationalIdCard(CancellationToken cancellationToken)
    {
        var personId = await _identityService.GetPersonIdFromClaimsAsync(User, cancellationToken);
        if (personId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Citizen security context invalid.", statusCode: 401));
        }

        var result = await _getActiveNationalIdCardHandler.GetByPersonIdAsync(personId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves active Passport details for the authenticated citizen
    /// </summary>
    [HttpGet("passport")]
    public async Task<IActionResult> GetMyPassport(CancellationToken cancellationToken)
    {
        var personId = await _identityService.GetPersonIdFromClaimsAsync(User, cancellationToken);
        if (personId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Citizen security context invalid.", statusCode: 401));
        }

        var result = await _getActivePassportHandler.GetByPersonIdAsync(personId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves all Birth Certificates for children of the authenticated citizen father
    /// </summary>
    [HttpGet("children-birth-certificates")]
    public async Task<IActionResult> GetMyChildrenBirthCertificates(CancellationToken cancellationToken)
    {
        var nationalNumber = await _identityService.GetNationalNumberFromClaimsAsync(User, cancellationToken);
        if (string.IsNullOrWhiteSpace(nationalNumber))
        {
            return Unauthorized(ApiResponse<object>.Failure("Citizen security context invalid.", statusCode: 401));
        }

        var result = await _getBirthCertificatesByFatherHandler.GetByFatherNationalNumberAsync(nationalNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves specific Birth Certificate details by Certificate ID
    /// </summary>
    [HttpGet("birth-certificates/{id:guid}")]
    public async Task<IActionResult> GetBirthCertificateById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getBirthCertificateByIdHandler.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves Death Certificates for all deceased family members of the authenticated citizen
    /// </summary>
    [HttpGet("death-certificates")]
    public async Task<IActionResult> GetMyFamilyDeathCertificates(CancellationToken cancellationToken)
    {
        var personId = await _identityService.GetPersonIdFromClaimsAsync(User, cancellationToken);
        if (personId == Guid.Empty)
        {
            return Unauthorized(ApiResponse<object>.Failure("Citizen security context invalid.", statusCode: 401));
        }

        var result = await _getFamilyDeathCertificatesHandler.GetFamilyDeathCertificatesAsync(personId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves full lifetime travel history for the authenticated citizen
    /// </summary>
    [HttpGet("travel-records")]
    public async Task<IActionResult> GetMyTravelHistory(CancellationToken cancellationToken)
    {
        var nationalNumber = await _identityService.GetNationalNumberFromClaimsAsync(User, cancellationToken);
        if (string.IsNullOrWhiteSpace(nationalNumber))
        {
            return Unauthorized(ApiResponse<object>.Failure("Citizen security context invalid.", statusCode: 401));
        }

        var result = await _getPersonTravelHistoryHandler.GetTravelHistoryByNationalNumberAsync(nationalNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
