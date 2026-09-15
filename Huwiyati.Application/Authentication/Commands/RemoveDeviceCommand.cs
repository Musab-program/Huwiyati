namespace Huwiyati.Application.Authentication.Commands;

public class RemoveDeviceCommand
{
    public string NationalNumber { get; set; } = string.Empty;
    public string DeviceIdentifier { get; set; } = string.Empty;
}