namespace Huwiyati.Application.Common.Models;

public class ServicePayloadValidationModel
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public int StatusCode { get; set; } = 400;
    public string? SerializedJson { get; set; }
}
