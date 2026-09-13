namespace Huwiyati.Application.Authentication.Commands;

public class VerifyDeviceCommand
{
    public string NationalNumber { get; set; } = string.Empty;
    public string DeviceIdentifier { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}