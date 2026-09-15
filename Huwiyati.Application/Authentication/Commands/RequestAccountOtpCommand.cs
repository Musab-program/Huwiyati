namespace Huwiyati.Application.Authentication.Commands;

public class RequestAccountOtpCommand
{
    public string NationalNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
