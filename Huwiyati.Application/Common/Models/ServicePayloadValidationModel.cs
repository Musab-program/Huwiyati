namespace Huwiyati.Application.Common.Models;

/// <summary>
/// Encapsulates the schema validation results and normalized JSON representation for a service request payload.
/// </summary>
public class ServicePayloadValidationModel
{
    /// <summary>
    /// Gets or sets a value indicating whether the payload conforms to the required service schema.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the validation failure reason or error message.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code (defaults to 400 Bad Request if invalid).
    /// </summary>
    public int StatusCode { get; set; } = 400;

    /// <summary>
    /// Gets or sets the serialized, sanitized JSON string of the payload data.
    /// </summary>
    public string? SerializedJson { get; set; }
}
