namespace Huwiyati.Infrastructure.Services;

using System.Text.Json;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.BirthCertificate.Commands;
using Huwiyati.Application.Documents.DeathCertificate.Commands;
using Huwiyati.Application.Documents.NationalIdCard.Commands;
using Huwiyati.Application.Documents.Passport.Commands;
using Huwiyati.Application.Family.Commands;
using Huwiyati.Domain.Entities.Requests;

/// <summary>
/// Service coordinating the fulfillment of approved service requests by executing corresponding domain issuance or renewal handlers.
/// </summary>
public class ServiceRequestFulfillmentService : IServiceRequestFulfillmentService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly RenewNationalIdCardHandler _renewNationalIdHandler;
    private readonly RenewPassportHandler _renewPassportHandler;
    private readonly RenewFamilyCardHandler _renewFamilyCardHandler;
    private readonly IssueBirthCertificateHandler _issueBirthHandler;
    private readonly IssueDeathCertificateHandler _issueDeathHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRequestFulfillmentService"/> class.
    /// </summary>
    /// <param name="renewNationalIdHandler">The handler for renewing national identity cards.</param>
    /// <param name="renewPassportHandler">The handler for renewing passports.</param>
    /// <param name="renewFamilyCardHandler">The handler for renewing family record cards.</param>
    /// <param name="issueBirthHandler">The handler for issuing birth certificates.</param>
    /// <param name="issueDeathHandler">The handler for issuing death certificates.</param>
    public ServiceRequestFulfillmentService(
        RenewNationalIdCardHandler renewNationalIdHandler,
        RenewPassportHandler renewPassportHandler,
        RenewFamilyCardHandler renewFamilyCardHandler,
        IssueBirthCertificateHandler issueBirthHandler,
        IssueDeathCertificateHandler issueDeathHandler)
    {
        _renewNationalIdHandler = renewNationalIdHandler;
        _renewPassportHandler = renewPassportHandler;
        _renewFamilyCardHandler = renewFamilyCardHandler;
        _issueBirthHandler = issueBirthHandler;
        _issueDeathHandler = issueDeathHandler;
    }

    /// <summary>
    /// Fulfills a service request upon status transition to Issued, invoking domain document generation.
    /// </summary>
    /// <param name="request">The service request entity being completed.</param>
    /// <param name="serviceTypeCode">The service type code identifying the target fulfillment process.</param>
    /// <param name="nationalNumber">The national identification number of the applicant.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> indicating fulfillment outcome.</returns>
    public async Task<ApiResponse<bool>> FulfillDocumentAsync(
        ServiceRequest request,
        string serviceTypeCode,
        string nationalNumber,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.RequestDataJson))
        {
            return ApiResponse<bool>.Failure("Request payload data is missing. Cannot fulfill document issuance.", statusCode: 400);
        }

        try
        {
            switch (serviceTypeCode)
            {
                case "NATIONAL_ID_RENEWAL":
                    var nationalIdCmd = JsonSerializer.Deserialize<RenewNationalIdCardCommand>(request.RequestDataJson, JsonOptions);
                    if (nationalIdCmd == null)
                        return ApiResponse<bool>.Failure("Invalid payload format for National ID Renewal command.", statusCode: 400);

                    if (string.IsNullOrWhiteSpace(nationalIdCmd.NationalNumber))
                        nationalIdCmd.NationalNumber = nationalNumber;

                    if (nationalIdCmd.IssuingBranchId == Guid.Empty)
                        nationalIdCmd.IssuingBranchId = request.BranchId;

                    var idResult = await _renewNationalIdHandler.RenewAsync(nationalIdCmd, cancellationToken);
                    return idResult.IsSuccess
                        ? ApiResponse<bool>.Success(true, idResult.Message)
                        : ApiResponse<bool>.Failure(idResult.Message, idResult.Errors, idResult.StatusCode);

                case "PASSPORT_RENEWAL":
                    var passportCmd = JsonSerializer.Deserialize<RenewPassportCommand>(request.RequestDataJson, JsonOptions);
                    if (passportCmd == null)
                        return ApiResponse<bool>.Failure("Invalid payload format for Passport Renewal command.", statusCode: 400);

                    if (string.IsNullOrWhiteSpace(passportCmd.NationalNumber))
                        passportCmd.NationalNumber = nationalNumber;

                    if (passportCmd.IssuingBranchId == Guid.Empty)
                        passportCmd.IssuingBranchId = request.BranchId;

                    var passportResult = await _renewPassportHandler.RenewAsync(passportCmd, cancellationToken);
                    return passportResult.IsSuccess
                        ? ApiResponse<bool>.Success(true, passportResult.Message)
                        : ApiResponse<bool>.Failure(passportResult.Message, passportResult.Errors, passportResult.StatusCode);

                case "FAMILY_CARD_RENEWAL":
                    var familyCmd = JsonSerializer.Deserialize<RenewFamilyCardCommand>(request.RequestDataJson, JsonOptions);
                    if (familyCmd == null)
                        return ApiResponse<bool>.Failure("Invalid payload format for Family Card Renewal command.", statusCode: 400);

                    if (familyCmd.IssuingBranchId == Guid.Empty)
                        familyCmd.IssuingBranchId = request.BranchId;

                    var familyResult = await _renewFamilyCardHandler.RenewAsync(familyCmd, cancellationToken);
                    return familyResult.IsSuccess
                        ? ApiResponse<bool>.Success(true, familyResult.Message)
                        : ApiResponse<bool>.Failure(familyResult.Message, familyResult.Errors, familyResult.StatusCode);

                case "BIRTH_CERTIFICATE_REGISTRATION":
                    var birthCmd = JsonSerializer.Deserialize<IssueBirthCertificateCommand>(request.RequestDataJson, JsonOptions);
                    if (birthCmd == null)
                        return ApiResponse<bool>.Failure("Invalid payload format for Birth Registration command.", statusCode: 400);

                    if (birthCmd.HospitalBranchId == Guid.Empty)
                        birthCmd.HospitalBranchId = request.BranchId;

                    if (birthCmd.IssuingBranchId == Guid.Empty)
                        birthCmd.IssuingBranchId = request.BranchId;

                    var birthResult = await _issueBirthHandler.IssueAsync(birthCmd, cancellationToken);
                    return birthResult.IsSuccess
                        ? ApiResponse<bool>.Success(true, birthResult.Message)
                        : ApiResponse<bool>.Failure(birthResult.Message, birthResult.Errors, birthResult.StatusCode);

                case "DEATH_CERTIFICATE_REGISTRATION":
                    var deathCmd = JsonSerializer.Deserialize<IssueDeathCertificateCommand>(request.RequestDataJson, JsonOptions);
                    if (deathCmd == null)
                        return ApiResponse<bool>.Failure("Invalid payload format for Death Registration command.", statusCode: 400);

                    if (string.IsNullOrWhiteSpace(deathCmd.NationalNumber))
                        deathCmd.NationalNumber = nationalNumber;

                    if (deathCmd.HospitalBranchId == Guid.Empty)
                        deathCmd.HospitalBranchId = request.BranchId;

                    if (deathCmd.IssuingBranchId == Guid.Empty)
                        deathCmd.IssuingBranchId = request.BranchId;

                    var deathResult = await _issueDeathHandler.IssueAsync(deathCmd, cancellationToken);
                    return deathResult.IsSuccess
                        ? ApiResponse<bool>.Success(true, deathResult.Message)
                        : ApiResponse<bool>.Failure(deathResult.Message, deathResult.Errors, deathResult.StatusCode);

                default:
                    return ApiResponse<bool>.Success(true, "No automated document issuance mapped for this service type.");
            }
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Failure($"Error while fulfilling document issuance: {ex.Message}", statusCode: 500);
        }
    }
}
