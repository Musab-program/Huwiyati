namespace Huwiyati.Application.Common.Interfaces;

using Huwiyati.Application.Common;
using Huwiyati.Domain.Entities.Requests;

/// <summary>
/// Defines automatic document fulfillment workflow operations upon issuance of service requests.
/// </summary>
public interface IServiceRequestFulfillmentService
{
    /// <summary>
    /// Executes the underlying domain fulfillment logic (e.g. creating or renewing ID cards, passports, family cards)
    /// when a service request transitions to the Issued status.
    /// </summary>
    /// <param name="request">The service request entity being fulfilled.</param>
    /// <param name="serviceTypeCode">The service catalog type code.</param>
    /// <param name="nationalNumber">The national number of the applicant.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating fulfillment success or failure.</returns>
    Task<ApiResponse<bool>> FulfillDocumentAsync(
        ServiceRequest request,
        string serviceTypeCode,
        string nationalNumber,
        CancellationToken cancellationToken = default);
}
