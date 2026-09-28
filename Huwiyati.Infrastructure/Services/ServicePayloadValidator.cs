namespace Huwiyati.Infrastructure.Services;

using System.Text.Json;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Models;
using Huwiyati.Application.Documents.BirthCertificate.Commands;
using Huwiyati.Application.Documents.BirthCertificate.Validators;
using Huwiyati.Application.Documents.DeathCertificate.Commands;
using Huwiyati.Application.Documents.DeathCertificate.Validators;
using Huwiyati.Application.Documents.NationalIdCard.Commands;
using Huwiyati.Application.Documents.NationalIdCard.Validators;
using Huwiyati.Application.Documents.Passport.Commands;
using Huwiyati.Application.Documents.Passport.Validators;
using Huwiyati.Application.Family.Commands;
using Huwiyati.Application.Family.Validators;

public class ServicePayloadValidator : IServicePayloadValidator
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ServicePayloadValidationModel ValidatePayload(string serviceTypeCode, object? rawPayload)
    {
        if (rawPayload == null)
        {
            return new ServicePayloadValidationModel
            {
                IsValid = true,
                StatusCode = 200,
                SerializedJson = null
            };
        }

        try
        {
            string serializedJson;
            if (rawPayload is JsonElement element)
            {
                serializedJson = element.GetRawText();
            }
            else
            {
                serializedJson = JsonSerializer.Serialize(rawPayload, JsonOptions);
            }

            switch (serviceTypeCode)
            {
                case "NATIONAL_ID_RENEWAL":
                    var nationalIdCmd = JsonSerializer.Deserialize<RenewNationalIdCardCommand>(serializedJson, JsonOptions);
                    if (nationalIdCmd == null)
                    {
                        return new ServicePayloadValidationModel
                        {
                            IsValid = false,
                            StatusCode = 400,
                            ErrorMessage = "Invalid payload format for National ID Renewal service."
                        };
                    }

                    var idValResult = new RenewNationalIdCardCommandValidator().Validate(nationalIdCmd);
                    if (!idValResult.IsValid)
                    {
                        return new ServicePayloadValidationModel
                        {
                            IsValid = false,
                            StatusCode = 400,
                            ErrorMessage = string.Join("; ", idValResult.Errors.Select(e => e.ErrorMessage))
                        };
                    }
                    break;

                case "PASSPORT_RENEWAL":
                    var passportCmd = JsonSerializer.Deserialize<RenewPassportCommand>(serializedJson, JsonOptions);
                    if (passportCmd == null)
                    {
                        return new ServicePayloadValidationModel
                        {
                            IsValid = false,
                            StatusCode = 400,
                            ErrorMessage = "Invalid payload format for Passport Renewal service."
                        };
                    }

                    var passportValResult = new RenewPassportCommandValidator().Validate(passportCmd);
                    if (!passportValResult.IsValid)
                    {
                        return new ServicePayloadValidationModel
                        {
                            IsValid = false,
                            StatusCode = 400,
                            ErrorMessage = string.Join("; ", passportValResult.Errors.Select(e => e.ErrorMessage))
                        };
                    }
                    break;

                case "FAMILY_CARD_RENEWAL":
                    var familyCmd = JsonSerializer.Deserialize<RenewFamilyCardCommand>(serializedJson, JsonOptions);
                    if (familyCmd == null)
                    {
                        return new ServicePayloadValidationModel
                        {
                            IsValid = false,
                            StatusCode = 400,
                            ErrorMessage = "Invalid payload format for Family Card Renewal service."
                        };
                    }

                    var familyValResult = new RenewFamilyCardCommandValidator().Validate(familyCmd);
                    if (!familyValResult.IsValid)
                    {
                        return new ServicePayloadValidationModel
                        {
                            IsValid = false,
                            StatusCode = 400,
                            ErrorMessage = string.Join("; ", familyValResult.Errors.Select(e => e.ErrorMessage))
                        };
                    }
                    break;

                case "BIRTH_CERTIFICATE_REGISTRATION":
                    var birthCmd = JsonSerializer.Deserialize<IssueBirthCertificateCommand>(serializedJson, JsonOptions);
                    if (birthCmd == null)
                    {
                        return new ServicePayloadValidationModel
                        {
                            IsValid = false,
                            StatusCode = 400,
                            ErrorMessage = "Invalid payload format for Birth Registration service."
                        };
                    }

                    var birthValResult = new IssueBirthCertificateCommandValidator().Validate(birthCmd);
                    if (!birthValResult.IsValid)
                    {
                        return new ServicePayloadValidationModel
                        {
                            IsValid = false,
                            StatusCode = 400,
                            ErrorMessage = string.Join("; ", birthValResult.Errors.Select(e => e.ErrorMessage))
                        };
                    }
                    break;

                case "DEATH_CERTIFICATE_REGISTRATION":
                    var deathCmd = JsonSerializer.Deserialize<IssueDeathCertificateCommand>(serializedJson, JsonOptions);
                    if (deathCmd == null)
                    {
                        return new ServicePayloadValidationModel
                        {
                            IsValid = false,
                            StatusCode = 400,
                            ErrorMessage = "Invalid payload format for Death Registration service."
                        };
                    }

                    var deathValResult = new IssueDeathCertificateCommandValidator().Validate(deathCmd);
                    if (!deathValResult.IsValid)
                    {
                        return new ServicePayloadValidationModel
                        {
                            IsValid = false,
                            StatusCode = 400,
                            ErrorMessage = string.Join("; ", deathValResult.Errors.Select(e => e.ErrorMessage))
                        };
                    }
                    break;
            }

            return new ServicePayloadValidationModel
            {
                IsValid = true,
                StatusCode = 200,
                SerializedJson = serializedJson
            };
        }
        catch (JsonException ex)
        {
            return new ServicePayloadValidationModel
            {
                IsValid = false,
                StatusCode = 400,
                ErrorMessage = $"Malformed JSON payload structure: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new ServicePayloadValidationModel
            {
                IsValid = false,
                StatusCode = 400,
                ErrorMessage = $"Payload validation error: {ex.Message}"
            };
        }
    }
}
