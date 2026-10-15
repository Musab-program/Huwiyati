namespace Huwiyati.Application.Common.Interfaces;

using Huwiyati.Application.Common.Models;

/// <summary>
/// Defines schema and validation operations for service request dynamic payload data.
/// </summary>
public interface IServicePayloadValidator
{
    /// <summary>
    /// Validates and normalizes the raw service payload against the expected JSON schema for the specified service type.
    /// </summary>
    /// <param name="serviceTypeCode">The catalog code of the service type (e.g., "SRV_RENEW_ID").</param>
    /// <param name="rawPayload">The raw payload object or JSON string submitted with the request.</param>
    /// <returns>A validation result model containing validity status, serialized JSON, and error details.</returns>
    ServicePayloadValidationModel ValidatePayload(string serviceTypeCode, object? rawPayload);
}
