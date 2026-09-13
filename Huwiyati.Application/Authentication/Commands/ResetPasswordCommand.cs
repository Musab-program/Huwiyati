namespace Huwiyati.Application.Authentication.Commands;

public class ResetPasswordCommand
{
    public string NationalNumber { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}