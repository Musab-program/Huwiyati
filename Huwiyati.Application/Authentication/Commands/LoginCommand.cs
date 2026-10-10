namespace Huwiyati.Application.Authentication.Commands;

using System;

public class LoginCommand
{
    public string NationalNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string RequestedRole { get; set; } = string.Empty;
    public Guid? BranchId { get; set; }
    public string DeviceIdentifier { get; set; } = string.Empty;
    public string DeviceName { get; set; } = "Unknown Device";
    public string OperatingSystem { get; set; } = "Unknown OS";
}
