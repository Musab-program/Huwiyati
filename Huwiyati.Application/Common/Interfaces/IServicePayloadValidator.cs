namespace Huwiyati.Application.Common.Interfaces;

using Huwiyati.Application.Common.Models;

public interface IServicePayloadValidator
{
    ServicePayloadValidationModel ValidatePayload(string serviceTypeCode, object? rawPayload);
}
