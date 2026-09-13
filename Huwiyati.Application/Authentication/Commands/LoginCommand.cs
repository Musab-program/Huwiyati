namespace Huwiyati.Application.Authentication.Commands;

public class LoginCommand
{
    public string NationalNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DeviceIdentifier { get; set; } = string.Empty;
    public string DeviceName { get; set; } = "Unknown Device";
    public string OperatingSystem { get; set; } = "Unknown OS";
}
